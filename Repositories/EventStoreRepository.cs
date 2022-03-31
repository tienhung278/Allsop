using Allsop.Contracts;
using Allsop.Models;
using Allsop.Models.Entities;

namespace Allsop.Repositories
{
    public class EventStoreRepository : RepositoryBase<EventStore>, IEventStoreRepository
    {
        private readonly RepositoryContext context;
        
        public EventStoreRepository(RepositoryContext context) : base(context)
        {
            this.context = context;
        }

        public void CreateEventStore(EventStore eventStore)
        {
            eventStore.CreatedAt = DateTime.Now;
            Add(eventStore);
            context.SaveChanges();
        }

        public EventStore? GetEvent(int id)
        {
            return FindByCondition(e => e.Id == id).SingleOrDefault();
        }

        public PageInfo<EventStore> GetEventPageInfo(QueryParameter parameter)
        {
            return GetPageInfo(parameter);
        }

        public ICollection<EventStore> GetEvents(QueryParameter parameter)
        {
            return GetItemsByPage(FindAll().OrderByDescending(a => a.Id), parameter.PageNumber, parameter.PageSize).ToList();
        }
    }
}