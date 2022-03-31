using Allsop.Contracts;
using Allsop.Models;
using Allsop.Models.Entities;
using System.Text.Json;

namespace Allsop.Repositories
{
    public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
    {
        private readonly RepositoryContext context;
        private readonly IEventStoreRepository eventStore;

        public BrandRepository(RepositoryContext context,
                                IEventStoreRepository eventStore) : base(context)
        {
            this.context = context;
            this.eventStore = eventStore;
        }        

        public Brand? GetBrand(int id)
        {
            return FindByCondition(p => p.Id == id).SingleOrDefault();
        }

        public IQueryable<Brand> GetBrands()
        {
            return FindAll().OrderByDescending(b => b.Id);
        }

        public ICollection<Brand> GetBrands(QueryParameter parameter)
        {
            return GetItemsByPage(FindAll().OrderByDescending(b => b.Id), parameter.PageNumber, parameter.PageSize).ToList();
        }        

        public void CreateBrand(Brand brand, string userId)
        {
            brand.CreatedAt = DateTime.Now;
            brand.CreatedBy = userId;
            Add(brand);
            EventLog(EventType.Create, string.Empty, JsonSerializer.Serialize(brand), userId);
            context.SaveChanges();
        }

        public void DeleteBrand(Brand brand, string userId)
        {
            Delete(brand);
            EventLog(EventType.Delete, JsonSerializer.Serialize(brand), string.Empty, userId);
            context.SaveChanges();
        }

        public void UpdateBrand(Brand brand, string userId)
        {
            brand.ModifiedAt = DateTime.Now;
            brand.ModifiedBy = userId;
            var currentBrand = GetBrand(brand.Id);
            Update(brand);
            EventLog(EventType.Update, JsonSerializer.Serialize(currentBrand), JsonSerializer.Serialize(brand), userId);
            context.SaveChanges();
        }

        public PageInfo<Brand> GetBrandPageInfo(QueryParameter parameter)
        {
            return GetPageInfo(parameter);
        }

        private void EventLog(EventType eventType, string oldObj, string newObj, string userId)
        {
            var eventLog = new EventStore 
            {
                EventType = eventType,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            if (eventType == EventType.Create)
            {
                eventLog.NewContent = newObj;
            }
            else
            {
                eventLog.OldContent = oldObj;

                if (eventType == EventType.Update)
                {                    
                    eventLog.NewContent = newObj;
                }
            }

            eventStore.CreateEventStore(eventLog);
        }
    }
}