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
        [Inject] IBrandNameService? BrandService { get; set; }
        [Inject] IEComponentService? EComponentService { get; set; }
        [Inject] IAuthService? AuthService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        #endregion

        #region Model and List Initialization
        Ecomponent eComponentModel = new();

        List<LuBrandName> brands = [];
        List<Ecomponent> ecomponents = [];
        #endregion

        protected override async Task OnInitializedAsync()
        {
            AuthService!.OnChange += HandleAuthStateChange;

            if (AuthService?.IsAuthorized == true)
                await Task.CompletedTask;
            else NavigationManager.NavigateTo(AuthService?.IsAuthorized == true ? "/AccessDenied" : "/");
            await InvokeAsync(StateHasChanged);

            if (BrandService != null && EComponentService != null)
            {
                brands = await BrandService.GetBrandNames();
                ecomponents = await EComponentService.GetComponents();
            }
        }

        /// <summary>
        /// Handles changes in the authentication state by triggering a UI refresh.
        /// </summary>
        private void HandleAuthStateChange()
        {
            InvokeAsync(StateHasChanged);
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
        private async void UpsertComponent(Ecomponent? context)
        {
            if (context != null && EComponentService != null)
                eComponentModel = await EComponentService.GetComponent(context) ?? new Ecomponent();
            upsertVisible = true;
            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ViewDetails(Ecomponent context)
        {
            if (context != null && EComponentService != null)
                eComponentModel = await EComponentService.GetComponent(context) ?? new Ecomponent();
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
            eComponentModel = new();
        }
        #endregion
    }
}
