using BlueDragon.Services;
using Microsoft.AspNetCore.Components;

namespace BlueDragon.Account
{

    public partial class Inventory
    {
        [Inject] AuthService? AuthService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            AuthService!.OnChange += HandleAuthStateChange;

            //if (AuthService?.IsAuthorized == true)
            //    await Task.CompletedTask;
            //else NavigationManager.NavigateTo(AuthService?.IsAuthorized == true ? "/AccessDenied" : "/");
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Handles changes in the authentication state by triggering a UI refresh.
        /// </summary>
        private void HandleAuthStateChange()
        {
            InvokeAsync(StateHasChanged);
        }
    }
}
