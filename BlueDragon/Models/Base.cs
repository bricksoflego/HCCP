using System.ComponentModel.DataAnnotations;

namespace BlueDragon.Models;
/// <summary>
/// Represents a base model with input validation for a barcode string. 
/// The class contains two properties:
/// 1. `InputString`: A generic string input.
/// 2. `NumberString`: A barcode string that is validated to ensure it is numeric, exactly 13 digits long, and required. 
/// The validation attributes ensure that only valid barcodes are accepted by enforcing these constraints.
/// </summary>
public class Base
{
    public string InputString { get; set; } = string.Empty;

    [Required(ErrorMessage = "Barcode is required.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Only numbers are allowed.")]
    [StringLength(13, ErrorMessage = "Barcode length must be 13 digits.", ErrorMessageResourceName = null, ErrorMessageResourceType = null, MinimumLength = 13)]
    public string NumberString { get; set; } = string.Empty;
}