using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlueDragon.Account;
public partial class Dashboard
{
    #region Dependencies
    [Inject] internal ISnackbar? Snackbar { get; set; }
    [Inject] IAuthService? AuthService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] internal ISolutionService SolutionService { get; set; } = default!;
    [Inject] IAuditService AuditStateService { get; set; } = default!;
    [Inject] IHardwareService? HardwareService { get; set; }
    [Inject] ICableService? CableService { get; set; }
    [Inject] IEComponentService? EComponentService { get; set; }
    [Inject] IPeripheralService? PeripheralService { get; set; }
    [Inject] AppConfig? AppConfig { get; set; }
    #endregion

    #region Variable Initialization
    public string SwitchText => AuditStateService!.IsAuditInProgress ? "In Progress" : "Not Started";
    public bool AuditInProgress { get; set; } = false;
    public string A { get; set; } = "http://oxstudios.com";
    #endregion


    protected override async Task OnInitializedAsync()
    {
        AppConfig!.CurrentPageTitle = "Manager Dashboard";
        AuthService!.OnChange += HandleAuthStateChange;

        if (AuthService?.IsAuthorized == true && (AuthService.IsInRole("Admin") || AuthService.IsInRole("Manager")))
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

    /// <summary>
    /// Handles the toggle of the audit switch, starting or stopping the audit accordingly.
    /// </summary>
    /// <param name="newValue">The new state of the switch: true for audit in progress, false for audit stopped.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task OnSwitchChanged(bool newValue)
    {
        AuditStateService!.IsAuditInProgress = newValue;
        await (newValue ? BeginAudit() : StopAudit());

        StateHasChanged();
    }

    /// <summary>
    /// Initiates the audit mode, updates settings, and provides user feedback.
    /// </summary>
    /// <returns>Task representing the asynchronous operation.</returns>
    public async Task BeginAudit()
    {
        AuditInProgress = true;
        A = "The link to the will appear once the audit has stopped.";

        if (SolutionService != null)
        {
            await SolutionService.Upsert("Audit", true);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar?.Add("Audit Mode Entered!", Severity.Info);
        }
    }

    /// <summary>
    /// Ends the audit mode, updates settings, and provides user feedback.
    /// </summary>
    /// <returns>Task representing the asynchronous operation.</returns>
    public async Task StopAudit()
    {
        AuditInProgress = false;
        A = "http://oxstudios.com";
        InventoryAudit auditResults = new();

        if (HardwareService != null && CableService != null && EComponentService != null && PeripheralService != null)
        {
            auditResults.Hardwares = await HardwareService.GetHardware();
            auditResults.Cables = await CableService.GetCables();
            auditResults.Ecomponents = await EComponentService.GetComponents();
            auditResults.Peripherals = await PeripheralService.GetPeripherals();
        }

        ProcessAuditResults(auditResults);

        if (SolutionService != null)
        {
            await SolutionService.Upsert("Audit", false);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar?.Add("Audit Mode Not Running.", Severity.Success);
        }

    }

    /// <summary>
    /// Takes the stored results and outputs them to a file
    /// </summary>
    /// <param name="auditResults"></param>
    private void ProcessAuditResults(InventoryAudit auditResults)
    {
        // Do something with the auditResults, like saving them to the database or performing an analysis
        // This is just a placeholder for your custom logic
        // TO BE IMPLEMENTED AS A FEATURE IN SECOND RELEASE (2.0)
    }
}