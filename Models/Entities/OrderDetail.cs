using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.Entities
{
    public class OrderDetail : ModelBase
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Total
        {
            get { return Product != null ? Product.Price * Quantity : 0; }
        }

        public OrderDetail()
        {
            Product = new Product();
        }
    }
}