using BlueDragon.Models;
using BlueDragon.Pages;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using System.Security.Claims;

namespace BlueDragon.Account
{
    public partial class Dashboard
    {
        #region Dependencies
        [Inject] ISnackbar? Snackbar { get; set; }
        [Inject] AuthService? AuthService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] SolutionService SolutionService { get; set; } = default!;
        [Inject] private AuditStateService? AuditStateService { get; set; }
        #endregion

        #region Model and List Initialization
        #endregion

        protected override async Task OnInitializedAsync()
        {

            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;

            if (AuthService != null && AuthService.IsAuthorized)
            {
                if (!AuthService.IsInRole("Admin") && !AuthService.IsInRole("Manager"))
                {
                    NavigationManager.NavigateTo("/AccessDenied");
                    return;
                }
                await Task.CompletedTask;
            }
            else
            {
                // If the user is not authenticated, redirect them to the login page
                NavigationManager.NavigateTo("/");
            }

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task BeginAudit()
        {
            AuditInProgress = true;
            A = "The link to the will appear once the audit has stopped.";
            SolutionSetting setting = new()
            {
                Name = "Audit",
                IsEnabled = true
            };
            if (SolutionService != null)
            {
                await SolutionService.Upsert(setting.Name, setting.IsEnabled);
                Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar?.Add("Audit Mode Entered!", Severity.Info);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task StopAudit()
        {
            AuditInProgress = false;
            A = "http://oxstudios.com";
            SolutionSetting setting = new()
            {
                Name = "Audit",
                IsEnabled = false
            };
            if (SolutionService != null)
            {
                await SolutionService.Upsert(setting.Name, setting.IsEnabled);
                Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                Snackbar?.Add("Audit Mode Not Running.", Severity.Success);
            }
        }
    }
}
