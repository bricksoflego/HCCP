namespace BlueDragon.Services;
public interface IAuthService
{
    bool IsAuthorized { get; }
    List<string> UserRoles { get; set; }

    event Action? OnChange;

    Task Login(string userName, string password);
    bool IsInRole(string role);
    void Logout();
}