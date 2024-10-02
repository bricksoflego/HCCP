﻿namespace BlueDragon.Services
{
    public class AuditService() : IAuditService
    {
        public event Action? OnChange;

        private bool _isAuditInProgress;

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
