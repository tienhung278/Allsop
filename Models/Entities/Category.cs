using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.Entities
{
    public class Category : ModelBase
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
    }
}