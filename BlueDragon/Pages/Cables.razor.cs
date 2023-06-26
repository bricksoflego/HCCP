using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages
{
    public partial class Cables
    {
        #region Dependencies
        [Inject] ISnackbar? Snackbar { get; set; }
        [Inject] BrandNameService BrandService { get; set; } = new();
        [Inject] CableTypeService CableTypeService { get; set; } = new();
        [Inject] CableService CableService { get; set; } = new();
        #endregion

        #region Model and List Initialization
        Cable cableModel = new();

        List<LuBrandName> brands = new();
        List<LuCableType> cableTypes = new();
        List<Cable> cables = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            brands = await BrandService.GetBrandNames();
            cableTypes = await CableTypeService.GetCableTypes();
            cables = await CableService.GetCables();
        }

        #region Cables
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task SaveCable(EditContext context)
        {
            cableModel = (Cable)context.Model;
            await CableService.Upsert(cableModel);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar.Add("Cable Added", Severity.Success);
            cableModel = new();
            cables = await CableService.GetCables();
            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task DeleteCable(Cable model)
        {
            await CableService.Delete(model);
            cables = await CableService.GetCables();
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

        private async void UpsertCable(Cable? context)
        {
            if (context != null)
                cableModel = await CableService.GetCable(context) ?? new Cable();
            upsertVisible = true;

        }
        private async Task ViewDetails(Cable context)
        {
            if (context != null)
                cableModel = await CableService.GetCable(context) ?? new Cable();
            detailVisible = true;
        }

        private void Close()
        {
            upsertVisible = false;
            detailVisible = false;
            cableModel = new();
        }
        #endregion

    }
}
