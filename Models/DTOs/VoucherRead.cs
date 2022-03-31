namespace Allsop.Models.DTOs
{
    public class VoucherRead
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal? AmountOff { get; set; }
        public decimal? SpentAmount { get; set; }
        public decimal? PercentageOff { get; set; }
        public int? CategoryId { get; set; }
        public int? Quantity { get; set; }
    }
}