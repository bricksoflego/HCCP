using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDragon.Models;

public partial class LuCableType
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
}
