using Allsop.Models;
using Allsop.Models.Entities;

namespace Allsop.Contracts
{
    public interface IEventStoreRepository
    {
        ICollection<EventStore> GetEvents(QueryParameter parameter);
        EventStore? GetEvent(int id);
        void CreateEventStore(EventStore eventStore);
        PageInfo<EventStore> GetEventPageInfo(QueryParameter parameter);
    }
}