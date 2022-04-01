using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.DTOs
{
    public class OrderDetailRead
    {
        public int Id { get; set; }
        public ProductRead Product { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }

        public OrderDetailRead()
        {
            Product = new ProductRead();
        }
    }
}