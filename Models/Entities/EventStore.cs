using System.ComponentModel.DataAnnotations;

namespace Allsop.Models.Entities
{
    public class EventStore : ModelBase
    {
        [Required]
        public EventType EventType { get; set; }
        public string? OldContent { get; set; }
        public string? NewContent { get; set; }
    }

    public enum EventType
    {
        Create,
        Update,
        Delete
    }
}