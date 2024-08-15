using BlueDragon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleManager"></param>
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<IdentityRole>> GetRoleListAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }
    }
}
