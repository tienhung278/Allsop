using System.ComponentModel.DataAnnotations;
using Allsop.Models.Entities;

namespace Allsop.Models.DTOs
{
    public class ProductWrite
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [MaxLength(500)]
        public string? Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}