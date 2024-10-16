using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages;
public partial class Components
{
    #region Dependencies
    [Inject] ISnackbar? Snackbar { get; set; }
    [Inject] IBrandNameService? BrandService { get; set; }
    [Inject] IEComponentService? EComponentService { get; set; }
    [Inject] IAuthService? AuthService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    #endregion

    #region Model and List Initialization
    Ecomponent eComponentModel = new();
    Base baseModel = new();

    List<LuBrandName> brands = [];
    List<Ecomponent> ecomponents = [];
    #endregion

    protected override async Task OnInitializedAsync()
    {
        AuthService!.OnChange += HandleAuthStateChange;

        if (AuthService?.IsAuthorized == true)
            await Task.CompletedTask;
        else NavigationManager.NavigateTo(AuthService?.IsAuthorized == true ? "/AccessDenied" : "/");
        await InvokeAsync(StateHasChanged);

        if (BrandService != null && EComponentService != null)
        {
            brands = await BrandService.GetBrandNames();
            ecomponents = await EComponentService.GetComponents();
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

    #region eComponents
    /// <summary>
    /// Saves or updates a component model using the provided edit context. If the EComponentService is available,
    /// the method retrieves the component model from the context, upserts it via the service, and displays
    /// a success message using a Snackbar. After saving, it resets the model and updates the list of components.
    /// </summary>
    /// <param name="context">The EditContext containing the component model to save or update.</param>
    internal async Task SaveComponent(EditContext context)
    {
        if (EComponentService != null)
        {
            eComponentModel = (Ecomponent)context.Model;
            await EComponentService.Upsert(eComponentModel);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar.Add("Component Added", Severity.Success);
            eComponentModel = new();
            ecomponents = await EComponentService.GetComponents();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Deletes the specified component model using the EComponentService. After deletion, it refreshes
    /// the list of components and triggers a UI update.
    /// </summary>
    /// <param name="model">The component model to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal async Task DeleteComponent(Ecomponent model)
    {
        if (EComponentService != null)
        {
            await EComponentService.Delete(model);
            ecomponents = await EComponentService.GetComponents();
            StateHasChanged();
        }
    }
    #endregion

    #region Upsert Dialog
    internal bool upsertVisible;
    internal bool detailVisible;

    /// <summary>
    /// Retrieves the specified component model from the EComponentService if it exists; otherwise, initializes a new component model.
    /// Once the model is retrieved or created, it sets the visibility of the upsert dialog and updates the UI.
    /// </summary>
    /// <param name="context">The component model to upsert, or null if a new component should be created.</param>
    internal async void UpsertComponent(Ecomponent? context)
    {
        if (context != null && EComponentService != null)
            eComponentModel = await EComponentService.GetComponent(context) ?? new Ecomponent();
        upsertVisible = true;
        StateHasChanged();
    }

    /// <summary>
    /// Retrieves the details of a specified component model from the EComponentService. If the model is found,
    /// it sets the current component model, makes the detail view visible, and updates the UI.
    /// </summary>
    /// <param name="context">The component model whose details are to be viewed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    internal async Task ViewDetails(Ecomponent context)
    {
        if (context != null && EComponentService != null)
            eComponentModel = await EComponentService.GetComponent(context) ?? new Ecomponent();
        detailVisible = true;
        StateHasChanged();
    }
    #endregion

    /// <summary>
    /// Closes all visible dialogs and resets the current component and base models. Specifically, it hides the upsert, detail, barcode, and page information dialogs.
    /// </summary>
    internal void Close()
    {
        upsertVisible = false;
        detailVisible = false;
        barcodeDialogVisible = false;
        pageInfoVisible = false;
        eComponentModel = new();
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
    /// Validates the provided barcode scan. If the scan is a 13-digit numeric string, it sets the search string,
    /// closes any open dialogs, and triggers a UI update.
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