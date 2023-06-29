using Microsoft.AspNetCore.Components;
using System;

namespace BlueDragon.Services
{
    public class AuthService
    {
        private readonly UserService _userService;

        public AuthService(UserService userService)
        {
            _userService = userService;
        }


        public bool IsAuthorized { get; private set; }

        public event Action? OnChange;

        public async Task Login(string userName, string password)
        {
            bool authorized = await _userService.GetUserCredentials(userName, password);
            if (IsAuthorized != authorized)
            {
                IsAuthorized = authorized;
                NotifyStateChanged();
            }
        }

        public void Logout()
        {
            IsAuthorized = false;
            NotifyStateChanged();
        }


        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
