using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.DTOs
{
    public class BrandWrite
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
    }
}