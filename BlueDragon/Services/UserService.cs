using BlueDragon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services;
public class UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    /// <summary>
    /// Validates the provided username and password by checking if the user exists and the password is correct.
    /// Uses the UserManager to find the user and the SignInManager to verify the password.
    /// Returns true if the credentials are valid, otherwise false.
    /// </summary>
    /// <param name="username">The username to validate.</param>
    /// <param name="password">The password to validate.</param>
    /// <returns>A task that returns true if the credentials are valid, otherwise false.</returns>
    public async Task<bool> GetUserCredentials(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user != null)
            return await _signInManager.UserManager.CheckPasswordAsync(user, password);
        return false;
    }

    /// <summary>
    /// Retrieves a list of all users from the UserManager, ordered by their username.
    /// </summary>
    /// <returns>A task that returns a list of ApplicationUser objects sorted by username.</returns>
    public virtual async Task<List<ApplicationUser>> GetUserList()
    {
        var users = _userManager.Users.ToList();
        return await _userManager.Users.OrderBy(c => c.UserName).ToListAsync();
    }

    /// <summary>
    /// Retrieves the information of a specified user by their username using the UserManager. 
    /// If the user is found, it returns the user information; otherwise, it returns a new ApplicationUser instance.
    /// </summary>
    /// <param name="username">The username of the user to retrieve information for.</param>
    /// <returns>A task that returns the user's information, or a new ApplicationUser instance if the user is not found.</returns>
    public async Task<ApplicationUser> GetUserInformation(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null)
            return (ApplicationUser)user;
        return new();
    }

    /// <summary>
    /// Creates or updates a user based on the provided login, email, password, and status. If the status is true, 
    /// it attempts to create a new user; if false, it updates an existing user. The method checks for existing users 
    /// and handles success or failure scenarios accordingly.
    /// </summary>
    /// <param name="login">The username of the user to create or update.</param>
    /// <param name="email">The email of the user to create or update.</param>
    /// <param name="password">The password for the new user (only used during creation).</param>
    /// <param name="status">A flag indicating whether to create (true) or update (false) a user.</param>
    /// <returns>A task that returns an IdentityResult indicating the success or failure of the operation.</returns>
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

        if (status) // CREATING A NEW USER
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
        else // UPDATING AN EXISTING USER
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
    /// Deletes a user identified by their username. If the user is found, the method deletes them using the UserManager;
    /// otherwise, it returns a failure result indicating that the user was not found.
    /// </summary>
    /// <param name="userName">The username of the user to delete.</param>
    /// <returns>A task that returns an IdentityResult indicating the success or failure of the deletion.</returns>
    public async Task<IdentityResult> DeleteUser(string userName)
    {

        // FIND THE USER BY THEIR ID
        var selectedUser = await _userManager.FindByNameAsync(userName);
        if (selectedUser == null)
        {
            // HANDLE CASE WHERE USER IS NOT FOUND
            return IdentityResult.Failed(new IdentityError { Description = $"User with ID {userName} not found." });
        }

        // DELETE THE USER
        var result = await _userManager.DeleteAsync(selectedUser);
        return result;
    }

    /// <summary>
    /// Retrieves the list of roles assigned to the specified user by using the UserManager.
    /// </summary>
    /// <param name="user">The user for whom to retrieve the roles.</param>
    /// <returns>A task that returns a list of role names associated with the user.</returns>
    public virtual async Task<IList<string>> GetUserRoles(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    /// <summary>
    /// Adds the specified role to the given user. If the role does not exist, it returns a failure result. 
    /// Otherwise, it assigns the role to the user using the UserManager.
    /// </summary>
    /// <param name="user">The user to whom the role will be added.</param>
    /// <param name="roleName">The name of the role to add.</param>
    /// <returns>A task that returns an IdentityResult indicating the success or failure of the operation.</returns>
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
    /// Removes the specified role from the given user using the UserManager.
    /// </summary>
    /// <param name="user">The user from whom the role will be removed.</param>
    /// <param name="roleName">The name of the role to remove.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RemoveRoleFromUser(ApplicationUser user, string roleName)
    {
        await _userManager.RemoveFromRoleAsync(user, roleName);
    }
}