using Microsoft.AspNetCore.Components.Authorization;

namespace BlueDragon.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserService _userService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(UserService userService, AuthenticationStateProvider authenticationStateProvider)
        {
            _userService = userService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public virtual bool IsAuthorized { get; private set; }

        public List<string> UserRoles { get; set; } = new List<string>();

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
}
