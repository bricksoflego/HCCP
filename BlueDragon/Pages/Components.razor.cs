using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BlueDragon.Pages
{
    public partial class Components
    {
        #region Dependencies
        [Inject] ISnackbar? Snackbar { get; set; }
        [Inject] BrandNameService? BrandService { get; set; }
        [Inject] EComponentService? EComponentService { get; set; }
        [Inject] AuthService? AuthService { get; set; }
        #endregion

        #region Model and List Initialization
        Ecomponent eComponentModel = new();

        List<LuBrandName> brands = new();
        List<Ecomponent> ecomponents = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;

            if (BrandService != null && EComponentService != null)
            {
                brands = await BrandService.GetBrandNames();
                ecomponents = await EComponentService.GetComponents();
            }
        }

        #region eComponents
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task SaveComponent(EditContext context)
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task DeleteComponent(Ecomponent model)
        {
            if(EComponentService != null) { 
            await EComponentService.Delete(model);
            ecomponents = await EComponentService.GetComponents();
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
        private async void UpsertComponent(Ecomponent? context)
        {
            if (context != null && EComponentService != null)
                eComponentModel = await EComponentService.GetComponent(context) ?? new Ecomponent();
            upsertVisible = true;
        }
        private async Task ViewDetails(Ecomponent context)
        {
            if (context != null && EComponentService != null)
                eComponentModel = await EComponentService.GetComponent(context) ?? new Ecomponent();
            detailVisible = true;
        }
        private void Close()
        {
            upsertVisible = false;
            detailVisible = false;
            eComponentModel = new();
        }
        #endregion
    }
}
