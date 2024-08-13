using Microsoft.AspNetCore.Components.Authorization;
using System;

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

        public event Action? OnChange;

        public async Task Login(string userName, string password)
        {
            bool authorized = await _userService.GetUserCredentials(userName, password);
            if (IsAuthorized != authorized)
            {
                IsAuthorized = authorized;

                if (authorized)
                {
                    var customAuthProvider = (CustomAuthenticationStateProvider)_authenticationStateProvider;
                    var userInfo = new UserInfo { UserName = userName };
                    await customAuthProvider.MarkUserAsAuthenticated(userInfo);
                }

                NotifyStateChanged();
            }
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
