using BlueDragon.Data;
using BlueDragon.Models;
using BlueDragon.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using System.Security.Claims;

namespace BlueDragon.Account
{
    public partial class Lookups
    {
        #region Dependencies
        [Inject] ISnackbar? Snackbar { get; set; }
        [Inject] BrandNameService? BrandService { get; set; }
        [Inject] CableTypeService? CableTypeService { get; set; }
        [Inject] AuthService? AuthService { get; set; }
        [Inject] UserService? UserService { get; set; }
        [Inject] RoleService? RoleService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
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

        string tempPassword = string.Empty;
        bool newApplication;

        protected override async Task OnInitializedAsync()
        {
            if (AuthService != null)
                AuthService.OnChange += StateHasChanged;

            if (AuthService.IsAuthorized)
            {
                if (!AuthService.IsInRole("Admin"))
                {
                    NavigationManager.NavigateTo("/AccessDenied");
                    return;
                }

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
            else
            {
                // If the user is not authenticated, redirect them to the login page
                NavigationManager.NavigateTo("/");
            }
        }

        #region User Accounts
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async Task SaveUserAccount(EditContext context)
        {
            bool isValid = context.Validate();
            if (context != null && UserService != null && isValid)
            {
                Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                try
                {
                    applicationUserModel = (ApplicationUser)context.Model;

                    // Create or update the user
                    IdentityResult result = await UserService.UpsertUser(
                        applicationUserModel.UserName ?? string.Empty,
                        applicationUserModel.Email ?? string.Empty,
                        tempPassword,
                        newApplication);

                    if (result.Succeeded)
                    {
                        // Reload the user from the database to get a fully tracked entity
                        applicationUserModel = await UserService.GetUserInformation(applicationUserModel.UserName);

                        // Now proceed with role assignment
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

                        // Refresh the user list and include roles
                        users = await UserService.GetUserList();
                        foreach (var user in users)
                        {
                            user.UserRoles = (List<string>)await UserService.GetUserRoles(user);
                        }

                        // Reset the model for new entry
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
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task DeleteUserAccount(ApplicationUser user)
        {
            if (UserService != null)
            {
                await UserService.DeleteUser(user.UserName);
                users = await UserService.GetUserList();
                StateHasChanged();
            }
        }
        #endregion

        #region Brand Names
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="context"></param>
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
        /// 
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
        /// TODO: Need to see if there is a way to enforce/override 
        /// clearing fields with saved browser settings.
        /// It's ugluy and annoying
        /// </summary>
        /// <param name="context"></param>
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async void UpsertBrand(LuBrandName? context)
        {
            if (context != null && BrandService != null)
                brandNameModel = await BrandService.GetBrandName(context) ?? new LuBrandName();
            upsertBrandVisible = true;
            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async void UpsertCableType(LuCableType? context)
        {
            if (context != null && CableTypeService != null)
                cableTypeModel = await CableTypeService.GetCableType(context) ?? new LuCableType();
            upsertCableVisible = true;
            StateHasChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookupType"></param>
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
}

