namespace Allsop.Models.DTOs
{
    public class ProductRead
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public BrandRead? Brand { get; set; }
        public CategoryRead? Category { get; set; }
    }
}