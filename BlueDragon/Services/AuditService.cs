using BlueDragon.Models;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata.Ecma335;
using static MudBlazor.Icons.Custom;

namespace BlueDragon.Services
{
    public class AuditService
    {
        public event Action? OnChange;

        private bool _isAuditInProgress;

        /// <summary>
        /// 
        /// </summary>
        public bool IsAuditInProgress
        {
            get => _isAuditInProgress;
            set
            {
                if (_isAuditInProgress != value)
                {
                    _isAuditInProgress = value;
                    NotifyStateChanged();
                }
            }
        }
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
