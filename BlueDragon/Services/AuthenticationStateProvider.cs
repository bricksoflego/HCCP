using BlueDragon.Extensions;
using BlueDragon.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace BlueDragon.Services;
public class CustomAuthenticationStateProvider(IJSRuntime jsRuntime) : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    private AuthenticationState? _cachedAuthenticationState;

    /// <summary>
    /// Retrieves the current authentication state asynchronously. If a cached authentication state exists, it returns it. 
    /// Otherwise, it checks if JavaScript runtime is available (to avoid issues during prerendering) and retrieves the user data from local storage. 
    /// If valid user data is found, it creates a ClaimsIdentity with the user's name and roles; otherwise, it returns an anonymous authentication state.
    /// </summary>
    /// <returns>A task that returns the current AuthenticationState, either for a logged-in user or an anonymous user.</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // RETURN CACHED AUTHENTICATION STATE IF AVAILABLE
        if (_cachedAuthenticationState != null)
        {
            return _cachedAuthenticationState;
        }

        // CHECK IF JSRuntime IS AVAILABLE (TO AVOID ISSUES DURING PRERENDERING)
        if (!_jsRuntime.IsJSRuntimeAvailable())
        {
            // Return an empty state during prerendering
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymousUser);
        }

        // RETRIEVE THE USER DATA FROM LOCALSTORAGE VIA JAVASCRIPT INTEROP
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authUser");
        var identity = new ClaimsIdentity();

        var applicationUser = JsonSerializer.Deserialize<ApplicationUser>(json);
        if (applicationUser != null && !string.IsNullOrEmpty(applicationUser.UserName))
        {
            identity = new ClaimsIdentity(new[]
            {
                        new Claim(ClaimTypes.Name, applicationUser.UserName),
                    }.Concat(applicationUser.UserRoles.Select(role => new Claim(ClaimTypes.Role, role))),
            "customAuthType");
        }

        var user = new ClaimsPrincipal(identity);
        _cachedAuthenticationState = new AuthenticationState(user);

        return _cachedAuthenticationState;
    }

    /// <summary>
    /// Marks the specified user as authenticated by creating a ClaimsIdentity with the user's name and roles.
    /// The authentication state is cached and the AuthenticationStateChanged event is triggered. 
    /// The user's data is also stored in local storage for future retrieval.
    /// </summary>
    /// <param name="applicationUser">The user to mark as authenticated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task MarkUserAsAuthenticated(ApplicationUser applicationUser)
    {
        if (applicationUser != null && !string.IsNullOrEmpty(applicationUser.UserName))
        {
            var identity = new ClaimsIdentity(new[]
        {
                new Claim(ClaimTypes.Name, applicationUser.UserName),
            }.Concat(applicationUser.UserRoles.Select(role => new Claim(ClaimTypes.Role, role))),
        "customAuthType");

            var user = new ClaimsPrincipal(identity);
            _cachedAuthenticationState = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(_cachedAuthenticationState));

            var json = JsonSerializer.Serialize(applicationUser);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authUser", json);
        }
    }

    /// <summary>
    /// Marks the user as logged out by removing their data from local storage and resetting the authentication state to an empty identity.
    /// The AuthenticationStateChanged event is triggered to notify listeners of the state change.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task MarkUserAsLoggedOut()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authUser");

        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        _cachedAuthenticationState = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(_cachedAuthenticationState));
    }
}