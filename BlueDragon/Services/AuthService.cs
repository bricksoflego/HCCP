/// Other than updating to C#10, this is a built-in authentication

using Microsoft.AspNetCore.Components.Authorization;

namespace BlueDragon.Services;
public class AuthService(UserService userService, AuthenticationStateProvider authenticationStateProvider) : IAuthService
{
    private readonly UserService _userService = userService;
    private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

    public virtual bool IsAuthorized { get; private set; }

    public List<string> UserRoles { get; set; } = [];

    public event Action? OnChange;

    public async Task Login(string userName, string password)
    {
        bool authorized = await _userService.GetUserCredentials(userName, password);
        IsAuthorized = authorized;

        if (authorized)
        {
            var applicationUser = await _userService.GetUserInformation(userName);
            applicationUser.UserRoles = (List<string>)await _userService.GetUserRoles(applicationUser);

            var customAuthProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            await customAuthProvider.MarkUserAsAuthenticated(applicationUser);

            UserRoles = applicationUser.UserRoles;

            OnChange?.Invoke();
        }
        NotifyStateChanged();
    }

    public bool IsInRole(string role)
    {
        return UserRoles.Contains(role);
    }

    public async void Logout()
    {
        IsAuthorized = false;

        var customAuthProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
        await customAuthProvider.MarkUserAsLoggedOut();

        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}