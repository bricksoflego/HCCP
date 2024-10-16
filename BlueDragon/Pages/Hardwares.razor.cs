using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages;
public partial class Hardwares
{
    #region Dependencies
    [Inject] ISnackbar? Snackbar { get; set; }
    [Inject] IBrandNameService? BrandService { get; set; }
    [Inject] IHardwareService? HardwareService { get; set; }
    [Inject] IAuthService? AuthService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    #endregion

    #region Model and List Initialization
    Hardware hardwareModel = new();
    Base baseModel = new();

    List<LuBrandName> brands = [];
    List<Hardware> hardwares = [];
    #endregion

    protected override async Task OnInitializedAsync()
    {
        AuthService!.OnChange += HandleAuthStateChange;

        if (AuthService?.IsAuthorized == true)
            await Task.CompletedTask;
        else NavigationManager.NavigateTo(AuthService?.IsAuthorized == true ? "/AccessDenied" : "/");
        await InvokeAsync(StateHasChanged);

        if (BrandService != null && HardwareService != null)
        {
            brands = await BrandService.GetBrandNames();
            hardwares = await HardwareService.GetHardware();
        }
    }

    /// <summary>
    /// Handles changes in the authentication state by triggering a UI refresh.
    /// </summary>
    private void HandleAuthStateChange()
    {
        InvokeAsync(StateHasChanged);
    }

    #region Hardware
    /// <summary>
    /// Saves or updates a hardware model using the provided edit context. If the HardwareService is available,
    /// it attempts to upsert the hardware model and displays a success message via the Snackbar. If an error occurs,
    /// an error message is shown instead. After saving, the hardware list is refreshed, and the UI is updated.
    /// </summary>
    /// <param name="context">The EditContext containing the hardware model to save or update.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal async Task SaveHardware(EditContext context)
    {
        if (HardwareService != null)
        {
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            try
            {
                hardwareModel = (Hardware)context.Model;
                await HardwareService.Upsert(hardwareModel);
                Snackbar.Add("Hardware Saved", Severity.Success);
                hardwareModel = new();
                hardwares = await HardwareService.GetHardware();
                StateHasChanged();
            }
            catch (Exception e)
            {
                Snackbar.Add("Hardware Not Saved", Severity.Error);
                Console.WriteLine(e);
            }
        }
    }

    /// <summary>
    /// Deletes the specified hardware model using the HardwareService. After deletion, the hardware list is refreshed, and the UI is updated.
    /// </summary>
    /// <param name="model">The hardware model to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal async Task DeleteHardware(Hardware model)
    {
        if (HardwareService != null)
        {
            await HardwareService.Delete(model);
            hardwares = await HardwareService.GetHardware();
            StateHasChanged();
        }
    }
    #endregion

    /// <summary>
    /// Defines static dialog options with full width, a close button, and disabled backdrop click.
    /// The dialog is positioned at the top center and has a maximum width of small.
    /// </summary>
    internal static readonly DialogOptions dialogOptions = new()
    {
        FullWidth = true,
        CloseButton = true,
        BackdropClick = false,
        Position = DialogPosition.TopCenter,
        MaxWidth = MaxWidth.Small
    };

    /// <summary>
    /// Defines static dialog options with full width, a close button, and disabled backdrop click.
    /// The dialog is positioned at the top center and has a maximum width of medium.
    /// </summary>
    internal static readonly DialogOptions largeDialogOptions = new()
    {
        FullWidth = true,
        CloseButton = true,
        BackdropClick = false,
        Position = DialogPosition.TopCenter,
        MaxWidth = MaxWidth.Medium
    };

    /// <summary>
    /// Defines static dialog options with full width, a close button, and disabled backdrop click.
    /// The dialog is positioned at the top center and has a maximum width of extra small, suitable for compact displays.
    /// </summary>
    internal static readonly DialogOptions compactDialogOptions = new()
    {
        FullWidth = true,
        CloseButton = true,
        BackdropClick = false,
        Position = DialogPosition.TopCenter,
        MaxWidth = MaxWidth.ExtraSmall
    };

    #region Upsert Dialog
    internal bool upsertVisible;
    internal bool detailVisible;

    /// <summary>
    /// Retrieves the specified hardware model from the HardwareService if it exists; otherwise, initializes a new hardware model.
    /// Once the model is retrieved or created, it sets the visibility of the upsert dialog and updates the UI.
    /// </summary>
    /// <param name="context">The hardware model to upsert, or null if a new hardware should be created.</param>
    internal async void UpsertHardware(Hardware? context)
    {
        if (context != null && HardwareService != null)
            hardwareModel = await HardwareService.GetSelectedHardware(context) ?? new Hardware();
        upsertVisible = true;
        StateHasChanged();
    }

    /// <summary>
    /// Retrieves the details of a specified hardware model from the HardwareService. If the model is found,
    /// it sets the current hardware model, makes the detail view visible, and updates the UI.
    /// </summary>
    /// <param name="context">The hardware model whose details are to be viewed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal async Task ViewDetails(Hardware context)
    {
        if (context != null && HardwareService != null)
            hardwareModel = await HardwareService.GetSelectedHardware(context) ?? new Hardware();
        detailVisible = true;
        StateHasChanged();
    }
    #endregion

    /// <summary>
    /// Closes all visible dialogs and resets the current hardware and base models. Specifically, it hides the upsert, detail, barcode, and page information dialogs.
    /// </summary>
    internal void Close()
    {
        upsertVisible = false;
        detailVisible = false;
        barcodeDialogVisible = false;
        pageInfoVisible = false;
        hardwareModel = new();
        baseModel = new();
    }

    internal bool barcodeDialogVisible;
    internal bool pageInfoVisible;

    /// <summary>
    /// Displays the barcode dialog by setting its visibility to true and triggers a UI update asynchronously.
    /// </summary>
    internal async void ShowBarcodeDialog()
    {
        barcodeDialogVisible = true;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Validates the provided barcode scan. If the scan is a valid 13-digit numeric string, 
    /// it sets the search string, closes any open dialogs, and triggers a UI update.
    /// </summary>
    /// <param name="scan">The scanned barcode string to validate and process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal async Task BarcodeLookupChanged(string scan)
    {
        if (scan.Length == 13 && Int64.TryParse(scan, out _) == true)
        {
            _searchString = scan;
            Close();
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Displays the page information by setting its visibility to true and triggers a UI update asynchronously.
    /// </summary>
    internal async void GetPageInfo()
    {
        pageInfoVisible = true;
        await InvokeAsync(StateHasChanged);
    }
}