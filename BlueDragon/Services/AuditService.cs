using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using static MudBlazor.Icons.Custom;

namespace BlueDragon.Services
{
    public class AuditService
    {
        private readonly HccContext _context;
        private readonly ILogger<AuditService> _logger;
        public AuditService(HccContext context, ILogger<AuditService> logger)
        {
            _context = context;
            _logger = logger;
        }
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
