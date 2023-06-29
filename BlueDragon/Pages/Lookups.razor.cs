using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages
{
    public partial class Lookups
    {
        #region Dependencies
        [Inject] ISnackbar? Snackbar { get; set; }
        [Inject] BrandNameService? BrandService { get; set; }
        [Inject] CableTypeService? CableTypeService { get; set; }
        #endregion

        #region Model and List Initialization
        LuBrandName brandNameModel = new();
        LuCableType cableTypeModel = new();

        List<LuBrandName> brands = new();
        List<LuCableType> cableTypes = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (BrandService != null && CableTypeService != null)
            {
                brands = await BrandService.GetBrandNames();
                cableTypes = await CableTypeService.GetCableTypes();
            }
        }

        #region Brand Names
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task SaveBrandName(EditContext context)
        {
            if (BrandService != null)
            {
                brandNameModel = (LuBrandName)context.Model;
                await BrandService.Upsert(brandNameModel);
                Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Brand Added", Severity.Success);
                brandNameModel = new();
                brands = await BrandService.GetBrandNames();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task DeleteBrand(LuBrandName model)
        {
            if (BrandService != null)
            {
                await BrandService.Delete(model);
                brands = await BrandService.GetBrandNames();
                StateHasChanged();
            }
        }
        #endregion

        #region Cable Types
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task SaveCableType(EditContext context)
        {
            if (CableTypeService != null)
            {
                cableTypeModel = (LuCableType)context.Model;
                await CableTypeService.Upsert(cableTypeModel);
                Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Cable Type Added", Severity.Success);
                cableTypeModel = new();
                cableTypes = await CableTypeService.GetCableTypes();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task DeleteCableType(LuCableType model)
        {
            if (CableTypeService != null)
            {
                await CableTypeService.Delete(model);
                cableTypes = await CableTypeService.GetCableTypes();
                StateHasChanged();
            }
        }
        #endregion

        #region Upsert Dialog
        private bool upsertBrandVisible;
        private bool upsertCableVisible;
        private DialogOptions dialogOptions = new()
        {
            FullWidth = true,
            CloseButton = true,
            DisableBackdropClick = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Small
        };

        private async void UpsertBrand(LuBrandName? context)
        {
            if (context != null && BrandService != null)
                brandNameModel = await BrandService.GetBrandName(context) ?? new LuBrandName();
            upsertBrandVisible = true;
        }
        private async void UpsertCableType(LuCableType? context)
        {
            if (context != null && CableTypeService != null)
                cableTypeModel = await CableTypeService.GetCableType(context) ?? new LuCableType();
            upsertCableVisible = true;
        }
        private void Close(string lookupType)
        {
            if (lookupType.Equals("brand"))
            {
                upsertBrandVisible = false;
                brandNameModel.Name = string.Empty;
            }
            else
            {
                upsertCableVisible = false;
                cableTypeModel.Name = string.Empty;
            }
        }
        #endregion
    }
}

