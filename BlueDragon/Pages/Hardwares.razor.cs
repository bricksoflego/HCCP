using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace BlueDragon.Pages
{
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task SaveHardware(EditContext context)
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task DeleteHardware(Hardware model)
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
        /// 
        /// </summary>
        private static readonly DialogOptions dialogOptions = new()
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
        private static readonly DialogOptions largeDialogOptions = new()
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
        private static readonly DialogOptions compactDialogOptions = new()
        {
            FullWidth = true,
            CloseButton = true,
            BackdropClick = false,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.ExtraSmall
        };

        #region Upsert Dialog
        private bool upsertVisible;
        private bool detailVisible;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async void UpsertHardware(Hardware? context)
        {
            if (context != null && HardwareService != null)
                hardwareModel = await HardwareService.GetSelectedHardware(context) ?? new Hardware();
            upsertVisible = true;
            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ViewDetails(Hardware context)
        {
            if (context != null && HardwareService != null)
                hardwareModel = await HardwareService.GetSelectedHardware(context) ?? new Hardware();
            detailVisible = true;
            StateHasChanged();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void Close()
        {
            upsertVisible = false;
            detailVisible = false;
            barcodeDialogVisible = false;
            pageInfoVisible = false;
            hardwareModel = new();
            baseModel = new();
        }

        private bool barcodeDialogVisible;
        private bool pageInfoVisible;

        /// <summary>
        /// 
        /// </summary>
        private async void ShowBarcodeDialog()
        {
            barcodeDialogVisible = true;
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        private async Task BarcodeLookupChanged(string scan)
        {
            if (scan.Length == 13 && Int64.TryParse(scan, out _) == true)
            {
                Close();
                await InvokeAsync(StateHasChanged);
            }
        }

        private async void GetPageInfo()
        {
            pageInfoVisible = true;
        }

    }
}
