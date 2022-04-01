using System.ComponentModel.DataAnnotations;

namespace Allsop.Models
{
    public class ModelBase
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public ModelBase()
        {
            CreatedBy = string.Empty;
            ModifiedBy = string.Empty;
        }
    }
}