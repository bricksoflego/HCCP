using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services;
public class RoleService(RoleManager<IdentityRole> roleManager)
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    /// <summary>
    /// Retrieves a list of all roles from the RoleManager asynchronously.
    /// </summary>
    /// <returns>A task that returns a list of IdentityRole objects.</returns>
    public virtual async Task<List<IdentityRole>> GetRoleListAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles;
    }
}