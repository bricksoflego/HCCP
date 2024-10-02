using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages
{
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

        #region Peripherals
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task SavePeripheral(EditContext context)
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
        private async Task DeletePeripheral(Peripheral model)
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
        private bool upsertVisible;
        private bool detailVisible;

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
        /// <param name="context"></param>
        private async void UpsertPeripheral(Peripheral? context)
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
        private async Task ViewDetails(Peripheral context)
        {
            if (context != null && PeripheralService != null)
                peripheralModel = await PeripheralService.GetPeripheral(context) ?? new Peripheral();
            detailVisible = true;
            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Close()
        {
            upsertVisible = false;
            detailVisible = false;
            peripheralModel = new();
        }
        #endregion

    }
}
