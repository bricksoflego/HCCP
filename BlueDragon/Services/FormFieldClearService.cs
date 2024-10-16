using BlueDragon.Models;

namespace BlueDragon.Services;
public class FormFieldClearService : IFormFieldClearService
{
    internal ApplicationUser applicationUserModel = new();
    internal LuBrandName brandNameModel = new();
    internal LuCableType cableTypeModel = new();
    internal string selectedRole = string.Empty;
    internal string tempPassword = string.Empty;

    /// <summary>
    /// Resets the user fields by clearing the current application user model, 
    /// resetting the selected role, and clearing the temporary password.
    /// </summary>
    public void ClearUserFields()
    {
        applicationUserModel = new ApplicationUser();
        selectedRole = string.Empty;
        tempPassword = string.Empty;
    }

    /// <summary>
    /// Resets the brand fields by clearing the current brand name model.
    /// </summary>
    public void ClearBrandFields()
    {
        brandNameModel = new LuBrandName();
    }

    /// <summary>
    /// Resets the cable fields by clearing the current cable type model.
    /// </summary>
    public void ClearCableFields()
    {
        cableTypeModel = new LuCableType();
    }
}