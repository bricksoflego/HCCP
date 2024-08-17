using BlueDragon.Models;

namespace BlueDragon.Services
{
    public class FormFieldClearService : IFormFieldClearService
    {
        private ApplicationUser applicationUserModel = new();
        private LuBrandName brandNameModel = new();
        private LuCableType cableTypeModel = new();
        private string selectedRole = string.Empty;
        private string tempPassword = string.Empty;

        public void ClearUserFields()
        {
            applicationUserModel = new ApplicationUser();
            selectedRole = string.Empty;
            tempPassword = string.Empty;
        }

        public void ClearBrandFields()
        {
            brandNameModel = new LuBrandName();
        }

        public void ClearCableFields()
        {
            cableTypeModel = new LuCableType();
        }
    }
}
