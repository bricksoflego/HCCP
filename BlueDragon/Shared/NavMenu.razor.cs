using BlueDragon.Components;
using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;

namespace BlueDragon.Shared
{
    public partial class NavMenu
    {
        #region Dependencies
        [Inject] private IAuthService? AuthService { get; set; }
        [Inject] private ApplicationUserService ApplicationUserService { get; set; } = default!;
        [Inject] private ISolutionService? SolutionService { get; set; }
        [Inject] private IAuditService? AuditStateService { get; set; }
        #endregion

        #region Model and List Initialization
        private List<SolutionSetting>? settings = [];
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;

            if (SolutionService != null)
            {
                settings = await SolutionService.GetSolutionSetings();
            }

            AuditStateService.OnChange += () =>
            {
                UpdateSettingsBasedOnAuditState();
                StateHasChanged();
            };

            ApplicationUserService.OnChange += HandleUserStateChange;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateSettingsBasedOnAuditState()
        {
            // Assuming settings is a list of SolutionSetting objects
            var auditSetting = settings.FirstOrDefault(x => x.Name == "Audit");
            if (auditSetting != null)
            {
                auditSetting.IsEnabled = AuditStateService.IsAuditInProgress;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleUserStateChange()
        {
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        public void HandleAuditStateChanged()
        {
            // RE - RENDER THE COMPONENT WHEN THE AUDIT STATE CHANGES
            InvokeAsync(StateHasChanged);
        }
    }
}
