using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BlueDragon.Models
{
    public class Base
    {
        public string InputString { get; set; } = string.Empty;

        [Required(ErrorMessage = "Barcode is required.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only numbers are allowed.")]
        [StringLength(13, ErrorMessage = "Barcode length must be 13 digits.", ErrorMessageResourceName = null, ErrorMessageResourceType = null, MinimumLength = 13)]
        public string NumberString { get; set; } = string.Empty;
    }
}
