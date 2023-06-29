using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages
{
    public partial class Hardwares
    {
        #region Dependencies
        [Inject] ISnackbar? Snackbar { get; set; }
        [Inject] BrandNameService? BrandService { get; set; }
        [Inject] HardwareService? HardwareService { get; set; }
        [Inject] AuthService? AuthService { get; set; }
        #endregion

        #region Model and List Initialization
        Hardware hardwareModel = new();

        List<LuBrandName> brands = new();
        List<Hardware> hardwares = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;

            if (BrandService != null && HardwareService != null)
            {
                brands = await BrandService.GetBrandNames();
                hardwares = await HardwareService.GetHardware();
            }
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
                hardwareModel = (Hardware)context.Model;
                await HardwareService.Upsert(hardwareModel);
                Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Hardware Added", Severity.Success);
                hardwareModel = new();
                hardwares = await HardwareService.GetHardware();
                StateHasChanged();
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

        #region Upsert Dialog
        private bool upsertVisible;
        private bool detailVisible;
        private DialogOptions dialogOptions = new()
        {
            FullWidth = true,
            CloseButton = true,
            DisableBackdropClick = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Small
        };
        private async void UpsertHardware(Hardware? context)
        {
            if (context != null && HardwareService != null)
                hardwareModel = await HardwareService.GetSelectedHardware(context) ?? new Hardware();
            upsertVisible = true;
        }

        private async Task ViewDetails(Hardware context)
        {
            if (context != null && HardwareService != null)
                hardwareModel = await HardwareService.GetSelectedHardware(context) ?? new Hardware();
            detailVisible = true;
        }
        private void Close()
        {
            upsertVisible = false;
            detailVisible = false;
            hardwareModel = new(); 
        }
        #endregion
    }
}
