using BlueDragon.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace BlueDragon.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private AuthenticationState? _cachedAuthenticationState;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Return cached authentication state if available
            if (_cachedAuthenticationState != null)
            {
                return _cachedAuthenticationState;
            }

            // Check if JSRuntime is available (to avoid issues during prerendering)
            if (!_jsRuntime.IsJSRuntimeAvailable())
            {
                // Return an empty state during prerendering
                var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonymousUser);
            }

            // Retrieve the user data from localStorage via JavaScript interop
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authUser");
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(json))
            {
                var userInfo = JsonSerializer.Deserialize<UserInfo>(json);
                if (userInfo != null)
                {
                    identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, userInfo.UserName),
                    }, "customAuthType");
                }
            }

            var user = new ClaimsPrincipal(identity);
            _cachedAuthenticationState = new AuthenticationState(user);

            return _cachedAuthenticationState;
        }

        public async Task MarkUserAsAuthenticated(UserInfo userInfo)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userInfo.UserName),
            }, "customAuthType");

            var user = new ClaimsPrincipal(identity);
            _cachedAuthenticationState = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(_cachedAuthenticationState));

            var json = JsonSerializer.Serialize(userInfo);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authUser", json);
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authUser");

            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            _cachedAuthenticationState = new AuthenticationState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(_cachedAuthenticationState));
        }
    }

    public class UserInfo
    {
        public string UserName { get; set; }
    }
}
