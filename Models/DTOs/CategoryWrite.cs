using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.DTOs
{
    public class CategoryWrite
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public CategoryWrite()
        {
            Name = string.Empty;
        }
    }
}