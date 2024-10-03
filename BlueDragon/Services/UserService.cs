using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlueDragon.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> GetUserCredentials(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
                return await _signInManager.UserManager.CheckPasswordAsync(user, password);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<ApplicationUser>> GetUserList()
        {
            var users = _userManager.Users.ToList();
            return await _userManager.Users.OrderBy(c => c.UserName).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetUserInformation(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
                return (ApplicationUser)user;
            return new();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IdentityResult> UpsertUser(string login, string email, string password, bool status)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(email))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Username and email must be provided."
                });
            }

            var existingUser = await _userManager.FindByNameAsync(login);

            IdentityResult result;

            if (status) // Creating a new user
            {
                if (existingUser != null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "Username already exists."
                    });
                }

                var newUser = new ApplicationUser { UserName = login, Email = email };
                result = await _userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    await _userManager.UpdateAsync(newUser);
                }
            }
            else // Updating an existing user
            {
                if (existingUser == null)
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "User not found."
                    });
                }

                existingUser.Email = email;
                result = await _userManager.UpdateAsync(existingUser);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteUser(string userName)
        {

            // Find the user by their ID
            var selectedUser = await _userManager.FindByNameAsync(userName);
            if (selectedUser == null)
            {
                // Handle case where user is not found
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID {userName} not found." });
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(selectedUser);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<IdentityResult> AddRoleToUser(ApplicationUser user, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' does not exist." });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task RemoveRoleFromUser(ApplicationUser user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }
    }
}
