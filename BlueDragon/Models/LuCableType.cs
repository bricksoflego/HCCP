using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDragon.Models;

/// <summary>
/// 
/// </summary>
public partial class LuCableType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
}
