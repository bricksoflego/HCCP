using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> GetUserCredentials(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
                return await _signInManager.UserManager.CheckPasswordAsync(user, password);
            return false;
        }

        public async Task<IdentityResult> CreateUser(string login, string email, string password)
        {
            var user = new ApplicationUser { UserName = login, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
                await _signInManager.SignInAsync(user, isPersistent: false);
            return result;
        }
    }
}
