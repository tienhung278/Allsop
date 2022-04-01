using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.Entities
{
    public class Brand : ModelBase
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public Brand()
        {
            Name = string.Empty;
        }
    }
}