namespace Allsop.Models.DTOs
{
    public class OrderRead
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public List<OrderDetailRead>? OrderDetails { get; set; }
        public decimal TotalDiscount { get; set; }
    }
}