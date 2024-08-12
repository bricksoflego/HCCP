using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
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

        List<LuBrandName> brands = [];
        List<Hardware> hardwares = [];
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

        #region Upsert Dialog
        private bool upsertVisible;
        private bool detailVisible;
        private static readonly DialogOptions dialogOptions = new()
        {
            FullWidth = true,
            CloseButton = true,
            BackdropClick = false,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Small
        };
        private async void UpsertHardware(Hardware? context)
        {
            if (context != null && HardwareService != null)
                hardwareModel = await HardwareService.GetSelectedHardware(context) ?? new Hardware();
            upsertVisible = true;
            StateHasChanged();
        }

        private async Task ViewDetails(Hardware context)
        {
            if (context != null && HardwareService != null)
                hardwareModel = await HardwareService.GetSelectedHardware(context) ?? new Hardware();
            detailVisible = true;
            StateHasChanged();
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
