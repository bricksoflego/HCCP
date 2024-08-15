using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDragon.Models;

/// <summary>
/// 
/// </summary>
public partial class Cable
{
    [Key]
    public Guid Cid { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string? BrandName { get; set; }
    [Required]
    public string? CableType { get; set; }
    [Required]
    public double? Length { get; set; }
    public string? Notes { get; set; }
    public bool Mutipart { get; set; }
    [Required]
    public int InUse { get; set; } = 0;
    [Required]
    public int OnHand { get; set; } = 0;
}
