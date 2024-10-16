using BlueDragon.Services;
using Microsoft.AspNetCore.Components;

namespace BlueDragon.Shared;
public partial class MainLayout
{
    [Inject] private IAuthService? AuthService { get; set; }

    protected override void OnInitialized()
    {
        if (AuthService != null)
            AuthService.OnChange += StateHasChanged;
    }

    private bool _drawerOpen = true;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}