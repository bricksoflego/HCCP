using BlueDragon.Services;
using Microsoft.AspNetCore.Components;

namespace BlueDragon.Account;
public partial class Inventory
{
    #region Dependencies
    [Inject] IAuthService? AuthService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] AppConfig? AppConfig { get; set; }
    #endregion

    protected override async Task OnInitializedAsync()
    {
        AppConfig!.CurrentPageTitle = "Inventory Audit";
        AuthService!.OnChange += HandleAuthStateChange;

        if (AuthService?.IsAuthorized == true)
            await Task.CompletedTask;
        else NavigationManager.NavigateTo(AuthService?.IsAuthorized == true ? "/AccessDenied" : "/");
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