using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlueDragon.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> GetUserCredentials(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
                return await _signInManager.UserManager.CheckPasswordAsync(user, password);
            return false;
        }

        public async Task<List<ApplicationUser>> GetUserList()
        {
            var users = _userManager.Users.ToList();
            return await _userManager.Users.OrderBy(c => c.UserName).ToListAsync();
        }
        public async Task<ApplicationUser> GetUserInformation(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
                return (ApplicationUser)user;
            return new();
        }

        public async Task<IdentityResult> UpsertUser(string login, string email, string password, bool status)
        {
            var result = new IdentityResult();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(email))
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Username and email must be provided."
                });
            }

            var existingUser = await _userManager.FindByNameAsync(login);

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

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task AddRoleToUser(ApplicationUser user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task RemoveRoleFromUser(ApplicationUser user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }
    }
}
