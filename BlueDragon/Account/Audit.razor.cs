using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using BlueDragon.Models;

namespace BlueDragon.Account
{
    public partial class Audit
    {
        #region Dependencies
        [Inject] HardwareService? HardwareService { get; set; }
        [Inject] CableService? CableService { get; set; }
        [Inject] EComponentService? EcomponentService { get; set; }
        [Inject] PeripheralService? PeripheralService { get; set; }
        #endregion

        #region Model and List Initialization
        private InventoryAudit? inventory = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            inventory.Hardwares = await HardwareService.GetHardware();
            inventory.Cables = await CableService.GetCables();
            inventory.Ecomponents = await EcomponentService.GetComponents();
            inventory.Peripherals = await PeripheralService.GetPeripherals();
            await Task.CompletedTask;
        }
    }
}
