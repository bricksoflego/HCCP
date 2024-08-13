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
        public ApplicationUser ApplicationUser { get; private set; } = new(); // Make applicationUser public

        [Inject] private UserService? UserService { get; set; }
        [Inject] private AuthService? AuthService { get; set; }
        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] private ApplicationUserService? ApplicationUserService { get; set; }

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
                        ApplicationUser = await UserService.GetUserInformation(login.UserName);
                        ApplicationUser.UserRoles = (List<string>)await UserService.GetUserRoles(ApplicationUser);
                    }
                    catch (Exception e)
                    {
                        // TODO:
                        Console.WriteLine(e);
                    }
                }
                ApplicationUserService?.UpdateUser(ApplicationUser);
                Close();
                await InvokeAsync(StateHasChanged);
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

        private async void Logout()
        {
            AuthService?.Logout();
            NavigationManager?.NavigateTo("/", true);
            // Clear the ApplicationUser in the service
            ApplicationUserService?.UpdateUser(new ApplicationUser());
            await InvokeAsync(StateHasChanged);
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
