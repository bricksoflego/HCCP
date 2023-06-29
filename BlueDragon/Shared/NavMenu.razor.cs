using BlueDragon.Services;
using Microsoft.AspNetCore.Components;

namespace BlueDragon.Shared
{
    public partial class NavMenu
    {
        [Inject] private AuthService? AuthService { get; set; }

        protected override void OnInitialized()
        {
            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;
        }
    }
}
