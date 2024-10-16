namespace BlueDragon.Services;
public class AuditService() : IAuditService
{
    public event Action? OnChange;

    private bool _isAuditInProgress;

    /// <summary>
    /// Gets or sets a value indicating whether an audit is currently in progress.
    /// Changes to this property will notify observers of the state change.
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