using BlueDragon.Models;
using System.Threading;

namespace BlueDragon.Services
{
    public class ApplicationUserService
    {
        public ApplicationUser ApplicationUser { get; private set; } = new ApplicationUser();

        public event Action? OnChange;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(ApplicationUser user)
        {
            ApplicationUser = user;
            NotifyStateChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
