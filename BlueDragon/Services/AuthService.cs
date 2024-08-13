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
        public List<string> UserRoles { get; set; } = new List<string>();

        public event Action? OnChange;

        public async Task Login(string userName, string password)
        {
            bool authorized = await _userService.GetUserCredentials(userName, password);
            IsAuthorized = authorized;

            if (authorized)
            {
                var customAuthProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
                var userInfo = new UserInfo { UserName = userName };
                await customAuthProvider.MarkUserAsAuthenticated(userInfo);
                // Fetch user roles after login
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity?.IsAuthenticated == true)
                {
                    UserRoles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                }

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
