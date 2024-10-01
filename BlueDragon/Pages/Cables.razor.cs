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
        [Inject] BrandNameService? BrandService { get; set; }
        [Inject] CableTypeService? CableTypeService { get; set; }
        [Inject] CableService? CableService { get; set; }
        [Inject] AuthService? AuthService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        #endregion

        #region Model and List Initialization
        Cable cableModel = new();

        List<LuBrandName> brands = [];
        List<LuCableType> cableTypes = [];
        List<Cable> cables = [];
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected string ConvertBool(bool b)
        {
            return b ? "Yes" : "No";
        }

        protected override async Task OnInitializedAsync()
        {
            AuthService!.OnChange += HandleAuthStateChange;

            if (AuthService?.IsAuthorized == true)
                await Task.CompletedTask;
            else NavigationManager.NavigateTo(AuthService?.IsAuthorized == true ? "/AccessDenied" : "/");
            await InvokeAsync(StateHasChanged);

            if (BrandService != null && CableTypeService != null && CableService != null)
            {
                brands = await BrandService.GetBrandNames();
                cableTypes = await CableTypeService.GetCableTypes();
                cables = await CableService.GetCables();
            }
        }

        /// <summary>
        /// Handles changes in the authentication state by triggering a UI refresh.
        /// </summary>
        private void HandleAuthStateChange()
        {
            InvokeAsync(StateHasChanged);
        }

        #region Cables
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task SaveCable(EditContext context)
        {
            if (CableService != null)
            {
                cableModel = (Cable)context.Model;
                await CableService.Upsert(cableModel);
                Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar.Add("Cable Added", Severity.Success);
                cableModel = new();
                cables = await CableService.GetCables();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task DeleteCable(Cable model)
        {
            if (CableService != null)
            {
                await CableService.Delete(model);
                cables = await CableService.GetCables();
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
        private async void UpsertCable(Cable? context)
        {
            if (context != null && CableService != null)
                cableModel = await CableService.GetCable(context) ?? new Cable();
            upsertVisible = true;
            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task ViewDetails(Cable context)
        {
            if (context != null && CableService != null)
                cableModel = await CableService.GetCable(context) ?? new Cable();
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
            cableModel = new();
        }
        #endregion
    }
}
