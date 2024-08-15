using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Data;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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

                        // Store roles in AuthService
                        AuthService.UserRoles = ApplicationUser.UserRoles;
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

        /// <summary>
        /// 
        /// </summary>
        private void Login()
        {
            loginDialogVisible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Close()
        {
            loginDialogVisible = false;

        }

        /// <summary>
        /// 
        /// </summary>
        private async void Logout()
        {
            AuthService?.Logout();
            NavigationManager?.NavigateTo("/", true);
            // Clear the ApplicationUser in the service
            ApplicationUserService?.UpdateUser(new ApplicationUser());
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// 
        /// </summary>
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
