using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlueDragon.Components
{
    public partial class UserLoginInformation
    {
        bool loginDialogVisible;
        LoginModel login = new();

        [Inject] private AuthService? AuthService { get; set; }

        [Inject] public NavigationManager? NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;
        }

        private async Task CheckAuthentication(EditContext context)
        {
            if (context != null && AuthService != null)
            {
                await AuthService.Login(login.UserName, login.Password);
                Close();
                StateHasChanged();
            }
        }

        private void Login()
        {
            loginDialogVisible = true;
        }

        private void Close()
        {
            loginDialogVisible = false;

        }

        private void Logout()
        {
            AuthService?.Logout();
            NavigationManager?.NavigateTo("/", true);
        }

        private DialogOptions dialogOptions = new()
        {
            FullWidth = true,
            CloseButton = true,
            DisableBackdropClick = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Small
        };
    }
}
