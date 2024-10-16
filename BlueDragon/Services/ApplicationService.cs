using BlueDragon.Models;

namespace BlueDragon.Services;
public class ApplicationUserService
{
    public ApplicationUser ApplicationUser { get; private set; } = new ApplicationUser();

    public event Action? OnChange;

    /// <summary>
    /// Updates the current user with the specified user object and notifies observers of the state change.
    /// </summary>
    /// <param name="user">The user object containing updated information.</param>
    public void UpdateUser(ApplicationUser user)
    {
        ApplicationUser = user;
        NotifyStateChanged();
    }

    /// <summary>
    /// Notifies observers that the state has changed by invoking the change event.
    /// </summary>
    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}