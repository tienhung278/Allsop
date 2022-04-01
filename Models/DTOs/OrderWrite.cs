using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.DTOs
{
    public class OrderWrite
    {
        public string VoucherCode { get; set; }

        public OrderWrite()
        {
            VoucherCode = string.Empty;
        }
    }
}