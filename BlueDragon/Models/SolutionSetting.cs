using System.ComponentModel.DataAnnotations;

namespace BlueDragon.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SolutionSetting
    {
        [Key]
        public Guid Sid { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public bool IsEnabled { get; set; }
    }
}
