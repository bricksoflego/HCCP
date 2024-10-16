using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages;
public partial class Peripherals
{
    #region Dependencies
    [Inject] ISnackbar? Snackbar { get; set; }
    [Inject] IBrandNameService? BrandService { get; set; }
    [Inject] IPeripheralService? PeripheralService { get; set; }
    [Inject] IAuthService? AuthService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; } = default!;
    #endregion

    #region Model and List Initialization
    Peripheral peripheralModel = new();
    Base baseModel = new();

    List<LuBrandName> brands = [];
    List<Peripheral> peripherals = [];
    #endregion

    protected override async Task OnInitializedAsync()
    {
        AuthService!.OnChange += HandleAuthStateChange;

        if (AuthService?.IsAuthorized == true)
            await Task.CompletedTask;
        else NavigationManager.NavigateTo(AuthService?.IsAuthorized == true ? "/AccessDenied" : "/");
        await InvokeAsync(StateHasChanged);

        if (BrandService != null && PeripheralService != null)
        {
            brands = await BrandService.GetBrandNames();
            peripherals = await PeripheralService.GetPeripherals();
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
    /// 
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
    /// 
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
    /// 
    /// </summary>
    internal static readonly DialogOptions compactDialogOptions = new()
    {
        FullWidth = true,
        CloseButton = true,
        BackdropClick = false,
        Position = DialogPosition.TopCenter,
        MaxWidth = MaxWidth.ExtraSmall
    };

    #region Peripherals
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    internal async Task SavePeripheral(EditContext context)
    {
        if (PeripheralService != null)
        {
            peripheralModel = (Peripheral)context.Model;
            await PeripheralService.Upsert(peripheralModel);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar.Add("Component Added", Severity.Success);
            peripheralModel = new();
            peripherals = await PeripheralService.GetPeripherals();
            StateHasChanged();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    internal async Task DeletePeripheral(Peripheral model)
    {
        if (PeripheralService != null)
        {
            await PeripheralService.Delete(model);
            peripherals = await PeripheralService.GetPeripherals();
            StateHasChanged();
        }
    }
    #endregion

    #region Upsert Dialog
    internal bool upsertVisible;
    internal bool detailVisible;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    internal async void UpsertPeripheral(Peripheral? context)
    {
        if (context != null && PeripheralService != null)
            peripheralModel = await PeripheralService.GetPeripheral(context) ?? new Peripheral();
        upsertVisible = true;
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    internal async Task ViewDetails(Peripheral context)
    {
        if (context != null && PeripheralService != null)
            peripheralModel = await PeripheralService.GetPeripheral(context) ?? new Peripheral();
        detailVisible = true;
        StateHasChanged();
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    internal void Close()
    {
        upsertVisible = false;
        detailVisible = false;
        barcodeDialogVisible = false;
        pageInfoVisible = false;
        peripheralModel = new();
        baseModel = new();
    }

    internal bool barcodeDialogVisible;
    internal bool pageInfoVisible;

    /// <summary>
    /// 
    /// </summary>
    internal async void ShowBarcodeDialog()
    {
        barcodeDialogVisible = true;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scan"></param>
    /// <returns></returns>
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
    /// 
    /// </summary>
    internal async void GetPageInfo()
    {
        pageInfoVisible = true;
        await InvokeAsync(StateHasChanged);
    }
}