﻿using BlueDragon.Data;
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
        [Inject] BrandNameService BrandService { get; set; } = new();
        [Inject] PeripheralService PeripheralService { get; set; } = new();
        #endregion

        #region Model and List Initialization
        Peripheral peripheralModel = new();

        List<LuBrandName> brands = new();
        List<Peripheral> peripherals = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            brands = await BrandService.GetBrandNames();
            peripherals = await PeripheralService.GetPeripherals();
        }

        #region Peripherals
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task SavePeripheral(EditContext context)
        {
            peripheralModel = (Peripheral)context.Model;
            await PeripheralService.Upsert(peripheralModel);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar.Add("Component Added", Severity.Success);
            peripheralModel = new();
            peripherals = await PeripheralService.GetPeripherals();
            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task DeletePeripheral(Peripheral model)
        {
            await PeripheralService.Delete(model);
            peripherals = await PeripheralService.GetPeripherals();
            StateHasChanged();
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
        private async void UpsertPeripheral(Peripheral? context)
        {
            if (context != null)
                peripheralModel = await PeripheralService.GetPeripheral(context) ?? new Peripheral();
            upsertVisible = true;
        }
        private async Task ViewDetails(Peripheral context)
        {
            if (context != null)
                peripheralModel = await PeripheralService.GetPeripheral(context) ?? new Peripheral();
            detailVisible = true;
        }
        private void Close()
        {
            upsertVisible = false;
            detailVisible = false;
            peripheralModel = new();
        }
        #endregion

    }
}
