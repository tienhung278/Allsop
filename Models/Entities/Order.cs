using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.Entities
{
    public class Order : ModelBase
    {        
        [Required]
        public decimal Total
        {
            get { return OrderDetails != null ? OrderDetails.Sum(od => od.Total) : 0; }
        }
        [Required]
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public string VoucherCode { get; set; }
        public int VoucherId { get; set; }
        public Voucher Voucher { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
            Voucher = new Voucher();
            VoucherCode = string.Empty;
        }
    }
}