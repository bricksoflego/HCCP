using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;

namespace BlueDragon.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(UserService userService, AuthenticationStateProvider authenticationStateProvider)
        {
            _userService = userService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public bool IsAuthorized { get; private set; }

        // Make the setter public so that roles can be assigned externally
        public List<string> UserRoles { get; set; } = new List<string>();

        public event Action? OnChange;

        public async Task Login(string userName, string password)
        {
            bool authorized = await _userService.GetUserCredentials(userName, password);
            IsAuthorized = authorized;

            if (authorized)
            {
                // Fetch the user's information, including roles
                var applicationUser = await _userService.GetUserInformation(userName);
                applicationUser.UserRoles = (List<string>)await _userService.GetUserRoles(applicationUser);

                // Pass the ApplicationUser object to the CustomAuthenticationStateProvider
                var customAuthProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
                await customAuthProvider.MarkUserAsAuthenticated(applicationUser);

                // Store roles locally in AuthService for quick access
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
