using BlueDragon.Components;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;

namespace BlueDragon.Shared
{
    public partial class NavMenu
    {
        [Inject] private AuthService? AuthService { get; set; }
        [Inject] private ApplicationUserService ApplicationUserService { get; set; } = new ApplicationUserService();

        protected override void OnInitialized()
        {
            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;

            ApplicationUserService.OnChange += HandleUserStateChange;
        }

        private async void HandleUserStateChange()
        {
            // Ensure UI update happens on the correct thread
            await InvokeAsync(StateHasChanged);
        }
    }
}
