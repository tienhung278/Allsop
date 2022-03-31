using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.DTOs
{
    public class OrderDetailWrite
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}