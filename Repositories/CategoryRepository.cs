using Allsop.Contracts;
using Allsop.Models;
using Allsop.Models.Entities;
using System.Text.Json;

namespace Allsop.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private readonly RepositoryContext context;
        private readonly IEventStoreRepository eventStore;

        public CategoryRepository(RepositoryContext context,
                                IEventStoreRepository eventStore) : base(context)
        {
            this.context = context;
            this.eventStore = eventStore;
        }        

        public Category? GetCategory(int id)
        {
            return FindByCondition(p => p.Id == id).SingleOrDefault();
        }

        public IQueryable<Category> GetCategories()
        {
            return FindAll().OrderByDescending(b => b.Id);
        }

        public ICollection<Category> GetCategories(QueryParameter parameter)
        {
            return GetItemsByPage(FindAll().OrderByDescending(b => b.Id), parameter.PageNumber, parameter.PageSize).ToList();
        }        

        public void CreateCategory(Category category, string userId)
        {
            category.CreatedAt = DateTime.Now;
            category.CreatedBy = userId;
            Add(category);
            EventLog(EventType.Create, string.Empty, JsonSerializer.Serialize(category), userId);
            context.SaveChanges();
        }

        public void DeleteCategory(Category category, string userId)
        {
            Delete(category);
            EventLog(EventType.Delete, JsonSerializer.Serialize(category), string.Empty, userId);
            context.SaveChanges();
        }

        public void UpdateCategory(Category category, string userId)
        {
            category.ModifiedAt = DateTime.Now;
            category.ModifiedBy = userId;
            var currentCategory = GetCategory(category.Id);
            Update(category);
            EventLog(EventType.Update, JsonSerializer.Serialize(currentCategory), JsonSerializer.Serialize(category), userId);
            context.SaveChanges();
        }

        public PageInfo<Category> GetCategoryPageInfo(QueryParameter parameter)
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