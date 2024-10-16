using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using MudBlazor;

namespace BlueDragon.Account;
public partial class Lookups
{
    #region Dependencies
    [Inject] ISnackbar? Snackbar { get; set; }
    [Inject] IBrandNameService? BrandService { get; set; }
    [Inject] ICableTypeService? CableTypeService { get; set; }
    [Inject] UserService? UserService { get; set; }
    [Inject] RoleService? RoleService { get; set; }
    [Inject] IAuthService? AuthService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    #endregion

    #region Model and List Initialization
    LuBrandName brandNameModel = new();
    LuCableType cableTypeModel = new();
    ApplicationUser applicationUserModel = new();

    List<LuBrandName> brands = [];
    List<LuCableType> cableTypes = [];
    List<ApplicationUser> users = [];
    List<IdentityRole> roles = [];
    string selectedRole = string.Empty;
    #endregion

    #region Model and List Initialization
    string tempPassword = string.Empty;
    bool newApplication;
    #endregion

    protected override async Task OnInitializedAsync()
    {
        AuthService!.OnChange += StateHasChanged;

        if (AuthService?.IsAuthorized == true && AuthService.IsInRole("Admin"))
            await Task.CompletedTask;
        else NavigationManager.NavigateTo(AuthService?.IsAuthorized == true ? "/AccessDenied" : "/");

        if (BrandService != null && CableTypeService != null && UserService != null && RoleService != null)
        {
            roles = await RoleService.GetRoleListAsync();
            brands = await BrandService.GetBrandNames();
            cableTypes = await CableTypeService.GetCableTypes();
            users = await UserService.GetUserList();

            foreach (var user in users)
            {
                user.UserRoles = (List<string>)await UserService.GetUserRoles(user);
            }
        }
    }

    #region User Accounts
    /// <summary>
    /// Saves or updates a user account and assigns roles.
    /// </summary>
    /// <param name="context">The edit context containing user data.</param> 
    private async Task SaveUserAccount(EditContext context)
    {
        bool isValid = context.Validate();
        if (context != null && UserService != null && isValid)
        {
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            try
            {
                applicationUserModel = (ApplicationUser)context.Model;

                // CREATE OR UPDATE THE USER
                IdentityResult result = await UserService.UpsertUser(
                    applicationUserModel.UserName ?? string.Empty,
                    applicationUserModel.Email ?? string.Empty,
                    tempPassword,
                    newApplication);

                if (result.Succeeded)
                {
                    // RELOAD THE USER FROM THE DATABASE TO GET A FULLY TRACKED ENTITY
                    applicationUserModel = await UserService.GetUserInformation(applicationUserModel.UserName!);

                    // NOW PROCEED WITH ROLE ASSIGNMENT
                    var currentRoles = await UserService.GetUserRoles(applicationUserModel);
                    foreach (var role in currentRoles)
                    {
                        await UserService.RemoveRoleFromUser(applicationUserModel, role);
                    }

                    if (!string.IsNullOrEmpty(selectedRole))
                    {
                        var addRoleResult = await UserService.AddRoleToUser(applicationUserModel, selectedRole);
                        if (!addRoleResult.Succeeded)
                        {
                            Console.WriteLine($"Failed to add role: {addRoleResult.Errors.FirstOrDefault()?.Description}");
                        }
                    }

                    Snackbar.Add("User Successfully Saved", Severity.Success);

                    // REFRESH THE USER LIST AND INCLUDE ROLES
                    users = await UserService.GetUserList();
                    foreach (var user in users)
                    {
                        user.UserRoles = (List<string>)await UserService.GetUserRoles(user);
                    }

                    // RESET THE MODEL FOR NEW ENTRY
                    applicationUserModel = new ApplicationUser();
                    selectedRole = string.Empty;
                    tempPassword = string.Empty;

                    context = new EditContext(applicationUserModel);

                    StateHasChanged();

                    newApplication = true;
                }
                else
                {
                    Snackbar.Add("User Not Saved", Severity.Error);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add("Error Occurred.", Severity.Error);
                Console.WriteLine(e);
            }
        }
    }

    /// <summary>
    /// Asynchronously deletes a user account from the system using the specified user object. 
    /// After deletion, it refreshes the list of users and updates the UI state.
    /// </summary>
    /// <param name="user">The user object representing the account to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task DeleteUserAccount(ApplicationUser user)
    {
        if (UserService != null)
        {
            await UserService.DeleteUser(user.UserName!);
            users = await UserService.GetUserList();
            StateHasChanged();
        }
    }
    #endregion

    #region Brand Names
    /// <summary>
    /// Asynchronously saves or updates the brand name based on the provided edit context.
    /// After saving, displays a success message via a snackbar notification, resets the brand model,
    /// refreshes the list of brand names, and updates the UI state.
    /// </summary>
    /// <param name="context">The EditContext containing the model data to be saved or updated.</param>
    private async Task SaveBrandName(EditContext context)
    {
        if (BrandService != null)
        {
            brandNameModel = (LuBrandName)context.Model;
            await BrandService.Upsert(brandNameModel);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar.Add("Brand Added", Severity.Success);
            brandNameModel = new();
            brands = await BrandService.GetBrandNames();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Asynchronously deletes the specified brand from the system. 
    /// After deletion, it refreshes the list of brand names and updates the UI state.
    /// </summary>
    /// <param name="model">The brand model representing the brand to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task DeleteBrand(LuBrandName model)
    {
        if (BrandService != null)
        {
            await BrandService.Delete(model);
            brands = await BrandService.GetBrandNames();
            StateHasChanged();
        }
    }
    #endregion

    #region Cable Types
    /// <summary>
    /// Asynchronously saves or updates the cable type based on the provided edit context. 
    /// After saving, displays a success message via a snackbar notification, resets the cable type model,
    /// refreshes the list of cable types, and updates the UI state.
    /// </summary>
    /// <param name="context">The EditContext containing the cable type model data to be saved or updated.</param>
    private async Task SaveCableType(EditContext context)
    {
        if (CableTypeService != null)
        {
            cableTypeModel = (LuCableType)context.Model;
            await CableTypeService.Upsert(cableTypeModel);
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
            Snackbar.Add("Cable Type Added", Severity.Success);
            cableTypeModel = new();
            cableTypes = await CableTypeService.GetCableTypes();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Asynchronously deletes the specified cable type from the system.
    /// After deletion, it refreshes the list of cable types and updates the UI state.
    /// </summary>
    /// <param name="model">The cable type model representing the cable type to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task DeleteCableType(LuCableType model)
    {
        if (CableTypeService != null)
        {
            await CableTypeService.Delete(model);
            cableTypes = await CableTypeService.GetCableTypes();
            StateHasChanged();
        }
    }
    #endregion

    #region Upsert Dialogs
    private bool upsertUserAccount;
    private bool upsertBrandVisible;
    private bool upsertCableVisible;

    /// <summary>
    /// Defines the configuration options for dialogs used in the application. 
    /// The dialog options include full-width display, a close button, disabling backdrop clicks to close the dialog,
    /// positioning the dialog at the top center of the screen, and setting the maximum width to small.
    /// </summary>
    private static readonly DialogOptions dialogOptions = new()
    {
        FullWidth = true,
        CloseButton = true,
        BackdropClick = false,
        Position = DialogPosition.TopCenter,
        MaxWidth = MaxWidth.Small
    };

    /// <summary>
    /// Asynchronously retrieves or initializes user information based on the provided context. 
    /// If the user exists, it fetches the user's roles and sets the selected role. 
    /// If the user does not exist, it initializes a new user model for creation.
    /// Finally, it opens the upsert dialog for user accounts and updates the UI state.
    /// </summary>
    /// <param name="context">The user context, either for an existing user or null for a new user.</param>
    private async Task UpsertUser(ApplicationUser? context)
    {
        selectedRole = string.Empty;
        if ((context?.UserName != null && context?.UserName != string.Empty) && UserService != null)
        {
            try
            {
                applicationUserModel = await UserService.GetUserInformation(context?.UserName ?? string.Empty);
                if (applicationUserModel.UserRoles.Count != 0)
                {
                    selectedRole = applicationUserModel.UserRoles.First();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        else
        {
            newApplication = true;
            applicationUserModel = new();
            tempPassword = string.Empty;
            selectedRole = string.Empty;
        }
        upsertUserAccount = true;
        StateHasChanged();
    }

    /// <summary>
    /// Asynchronously retrieves or initializes brand information based on the provided context. 
    /// If the brand exists, it fetches the brand data; otherwise, it initializes a new brand model. 
    /// Finally, it opens the upsert dialog for brands and updates the UI state.
    /// </summary>
    /// <param name="context">The brand context, either for an existing brand or null for a new brand.</param>
    private async void UpsertBrand(LuBrandName? context)
    {
        if (context != null && BrandService != null)
            brandNameModel = await BrandService.GetBrandName(context) ?? new LuBrandName();
        upsertBrandVisible = true;
        StateHasChanged();
    }

    /// <summary>
    /// Asynchronously retrieves or initializes cable type information based on the provided context. 
    /// If the cable type exists, it fetches the cable type data; otherwise, it initializes a new cable type model. 
    /// Finally, it opens the upsert dialog for cable types and updates the UI state.
    /// </summary>
    /// <param name="context">The cable type context, either for an existing cable type or null for a new cable type.</param>
    private async void UpsertCableType(LuCableType? context)
    {
        if (context != null && CableTypeService != null)
            cableTypeModel = await CableTypeService.GetCableType(context) ?? new LuCableType();
        upsertCableVisible = true;
        StateHasChanged();
    }

    /// <summary>
    /// Closes the upsert dialog for the specified lookup type and resets the corresponding model and related fields.
    /// This method handles user accounts, brands, and cable types, resetting the UI state as appropriate.
    /// </summary>
    /// <param name="lookupType">The type of dialog to close: "userAccount", "brand", or "cable".</param>
    private void Close(string lookupType)
    {
        switch (lookupType)
        {
            case "userAccount":
                upsertUserAccount = false;
                applicationUserModel = new();
                tempPassword = string.Empty;
                newApplication = false;
                selectedRole = string.Empty;
                break;
            case "brand":
                upsertBrandVisible = false;
                brandNameModel = new();
                break;
            case "cable":
                upsertCableVisible = false;
                cableTypeModel = new();
                break;
        }
    }
    #endregion
}