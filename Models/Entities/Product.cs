using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.Entities
{
    public class Product : ModelBase
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}