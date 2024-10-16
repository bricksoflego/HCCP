namespace BlueDragon.Services;
public interface IAuditService
{
    bool IsAuditInProgress { get; set; }
    event Action? OnChange;
}