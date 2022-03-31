namespace Allsop.Models.Entities
{
    public class Voucher : ModelBase
    {
        public string? Code { get; set; }
        public decimal? AmountOff { get; set; }
        public decimal? SpentAmount { get; set; }
        public decimal? PercentageOff { get; set; }
        public int? CategoryId { get; set; }
        public int? Quantity { get; set; }
    }
}