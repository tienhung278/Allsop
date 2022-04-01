using Allsop.Models.Entities;

namespace Allsop.Models.DTOs
{
    public class ActivityRead
    {
        public int Id { get; set; }
        public ProductProperty SearchType { get; set; }
        public string SearchValue { get; set; }
        public ProductProperty FilterType { get; set; }
        public int FilterValue { get; set; }
        public OrderType OrderType { get; set; }
        public ProductProperty OrderValue { get; set; }

        public ActivityRead()
        {
            SearchValue = string.Empty;
        }
    }
}