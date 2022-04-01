using Allsop.Models.Entities;

namespace Allsop.Models.DTOs
{
    public class EventStoreRead
    {
        public int Id { get; set; }
        public EventType EventType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string OldContent { get; set; }
        public string NewContent { get; set; }

        public EventStoreRead()
        {
            CreatedBy = string.Empty;
            OldContent = string.Empty;
            NewContent = string.Empty;
        }
    }
}