namespace Allsop.Models.DTOs
{
    public class OrderRead
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public ICollection<OrderDetailRead> OrderDetails { get; set; }
        public decimal TotalDiscount { get; set; }

        public OrderRead()
        {
            OrderDetails = new List<OrderDetailRead>();
        }
    }
}