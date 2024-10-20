﻿using System.ComponentModel.DataAnnotations;

namespace BlueDragon.Models;
public partial class LuBrandName
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
}