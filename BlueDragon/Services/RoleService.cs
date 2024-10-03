using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public virtual async Task<List<IdentityRole>> GetRoleListAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }
    }
}
