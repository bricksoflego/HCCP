﻿using System.ComponentModel.DataAnnotations;

namespace BlueDragon.Models;
public partial class Ecomponent
{
    [Key]
    public Guid Ecid { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string? BrandName { get; set; }
    public string? ModelNumber { get; set; }
    public string? SerialNumber { get; set; }
    [Required]
    public string? Location { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    [Required]
    public int InUse { get; set; } = 0;
    [Required]
    public int OnHand { get; set; } = 0;
    public string? Barcode { get; set; }

}