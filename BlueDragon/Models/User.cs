using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueDragon.Models
{
    public class LoginModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public List<string> UserRoles { get; set; } = new List<string>();
    }
}
