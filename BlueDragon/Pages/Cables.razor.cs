using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages;
public partial class Cables
{
    #region Dependencies
    [Inject] ISnackbar? Snackbar { get; set; }
    [Inject] IBrandNameService? BrandService { get; set; }
    [Inject] ICableTypeService? CableTypeService { get; set; }
    [Inject] ICableService? CableService { get; set; }
    [Inject] IAuthService? AuthService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    [Inject] AppConfig? AppConfig { get; set; }
    #endregion

    #region Model and List Initialization
    Cable cableModel = new();
    Base baseModel = new();

    List<LuBrandName> brands = [];
    List<LuCableType> cableTypes = [];
    List<Cable> cables = [];
    #endregion

    /// <summary>
    /// Converts a boolean value to its corresponding string representation.
    /// Returns "Yes" if the input is true, and "No" if the input is false.
    /// </summary>
    /// <param name="b">The boolean value to convert.</param>
    /// <returns>"Yes" if true, "No" if false.</returns>
    protected string ConvertBool(bool b)
    {
        return b ? "Yes" : "No";
    }

    protected override async Task OnInitializedAsync()
    {
        AppConfig!.CurrentPageTitle = "Cables";
        AuthService!.OnChange += HandleAuthStateChange;

        if (AuthService?.IsAuthorized == true)
            await Task.CompletedTask;
        else NavigationManager.NavigateTo(AuthService?.IsAuthorized == true ? "/AccessDenied" : "/");
        await InvokeAsync(StateHasChanged);

        if (BrandService != null && CableTypeService != null && CableService != null)
        {
            brands = await BrandService.GetBrandNames();
            cableTypes = await CableTypeService.GetCableTypes();
            cables = await CableService.GetCables();
        }
    }

    /// <summary>
    /// Handles changes in the authentication state by triggering a UI refresh.
    /// </summary>
    private void HandleAuthStateChange()
    {
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Defines static default options for a dialog. These options specify that the dialog
    /// will be full width, include a close button, disable closing by clicking the backdrop,
    /// be positioned at the top center, and have a maximum width of small.
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
    /// Defines static default options for a larger dialog. These options specify that the dialog
    /// will be full width, include a close button, disable closing by clicking the backdrop,
    /// be positioned at the top center, and have a maximum width of medium.
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
    /// Defines static default options for a compact dialog. These options specify that the dialog
    /// will be full width, include a close button, disable closing by clicking the backdrop,
    /// be positioned at the top center, and have a maximum width of extra small.
    /// </summary>
    internal static readonly DialogOptions compactDialogOptions = new()
    {
        FullWidth = true,
        CloseButton = true,
        BackdropClick = false,
        Position = DialogPosition.TopCenter,
        MaxWidth = MaxWidth.ExtraSmall
    };

    #region Cables
    /// <summary>
    /// Saves or updates a cable model using the provided edit context. If the CableService is available,
    /// the method retrieves the cable model from the context, upserts it via the service, and displays
    /// a success message using a Snackbar. After saving, it resets the model and updates the list of cables.
    /// </summary>
    /// <param name="context">The EditContext containing the cable model to save or update.</param>
    internal async Task SaveCable(EditContext context)
    {
        if (CableService != null)
        {
            cableModel = (Cable)context.Model;
            await CableService.Upsert(cableModel);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar.Add("Cable Added", Severity.Success);
            cableModel = new();
            cables = await CableService.GetCables();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Deletes a specified cable model using the CableService. After deletion, it refreshes
    /// the list of cables and triggers a UI update.
    /// </summary>
    /// <param name="model">The cable model to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal async Task DeleteCable(Cable model)
    {
        if (CableService != null)
        {
            await CableService.Delete(model);
            cables = await CableService.GetCables();
            StateHasChanged();
        }
    }
    #endregion

    #region Upsert Dialog
    internal bool upsertVisible;
    internal bool detailVisible;

    /// <summary>
    /// Retrieves the specified cable model from the CableService if it exists; otherwise, initializes a new cable model.
    /// Once the model is retrieved or created, it sets the visibility of the upsert dialog and updates the UI.
    /// </summary>
    /// <param name="context">The cable model to upsert, or null if a new cable should be created.</param>
    internal async void UpsertCable(Cable? context)
    {
        if (context != null && CableService != null)
            cableModel = await CableService.GetCable(context) ?? new Cable();
        upsertVisible = true;
        StateHasChanged();
    }

    /// <summary>
    /// Retrieves the details of a specified cable model from the CableService. If the model is found, 
    /// it sets the current cable model and makes the detail view visible, then updates the UI.
    /// </summary>
    /// <param name="context">The cable model whose details are to be viewed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal async Task ViewDetails(Cable context)
    {
        if (context != null && CableService != null)
            cableModel = await CableService.GetCable(context) ?? new Cable();
        detailVisible = true;
        StateHasChanged();
    }
    #endregion

    /// <summary>
    /// Closes all visible dialogs and resets the current cable and base models. Specifically, it hides the upsert, detail, barcode, and page information dialogs.
    /// </summary>
    private void Close()
    {
        upsertVisible = false;
        detailVisible = false;
        barcodeDialogVisible = false;
        pageInfoVisible = false;
        cableModel = new();
        baseModel = new();
    }

    internal bool barcodeDialogVisible;
    internal bool pageInfoVisible;

    /// <summary>
    /// Displays the barcode dialog by setting its visibility to true, then triggers a UI update.
    /// </summary>
    internal async void ShowBarcodeDialog()
    {
        barcodeDialogVisible = true;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles changes to the barcode lookup input. If the provided scan is a valid 13-digit numeric string, 
    /// it sets the search string, closes any open dialogs, and updates the UI.
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
    /// Displays the page information by setting its visibility to true, then triggers a UI update.
    /// </summary>
    internal async void GetPageInfo()
    {
        pageInfoVisible = true;
        await InvokeAsync(StateHasChanged);
    }
}