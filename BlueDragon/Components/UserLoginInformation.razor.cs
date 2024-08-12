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
        readonly LoginModel login = new();
        ApplicationUser applicationUser = new();

        [Inject] private UserService? UserService { get; set; }

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

                if (AuthService.IsAuthorized && UserService != null)
                {
                    try
                    {
                        applicationUser = await UserService.GetUserInformation(login.UserName);
                    }
                    catch (Exception e)
                    {
                        // TODO:
                        Console.WriteLine(e);
                    }
                }
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

        private static readonly DialogOptions dialogOptions = new()
        {
            FullWidth = true,
            CloseButton = true,
            BackdropClick = false,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Small
        };
    }
}
