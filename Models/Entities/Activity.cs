namespace Allsop.Models.Entities
{
    public class Activity : ModelBase
    {
        public ProductProperty SearchType { get; set; }
        public string SearchValue { get; set; }
        public ProductProperty FilterType { get; set; }
        public int FilterValue { get; set; }
        public OrderType OrderType { get; set; }
        public ProductProperty OrderValue { get; set; }
        
        public Activity()
        {
            SearchValue = string.Empty;
        }
    }    

    public enum ProductProperty
    {
        Brand,
        Price,
        Name,
        Id,
        Category
    }

    public enum OrderType
    {
        Ascending,
        Descending
    }
}