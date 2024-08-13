using BlueDragon.Models;
using System.Threading;

namespace BlueDragon.Services
{
    public class ApplicationUserService
    {
        public ApplicationUser ApplicationUser { get; private set; } = new ApplicationUser();

        public event Action? OnChange;

        public void UpdateUser(ApplicationUser user)
        {
            ApplicationUser = user;
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
