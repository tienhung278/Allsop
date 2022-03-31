using Microsoft.EntityFrameworkCore;
using Allsop.Contracts;
using Allsop.Models;
using Allsop.Models.Entities;
using System.Text.Json;
using Allsop.Models.DTOs;

namespace Allsop.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly RepositoryContext context;
        private readonly IActivityRepository activity;
        private readonly IEventStoreRepository eventStore;
        private readonly IBrandRepository brand;
        private readonly ICategoryRepository category;

        public ProductRepository(RepositoryContext context,
                                IBrandRepository brand,
                                ICategoryRepository category, 
                                IActivityRepository activity,
                                IEventStoreRepository eventStore) : base(context)
        {
            this.context = context;
            this.activity = activity;
            this.eventStore = eventStore;
            this.brand = brand;
            this.category = category;
        }        

        public Product? GetProduct(int id)
        {
            CollectActivities(id);

            return FindByCondition(p => p.Id == id)
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .SingleOrDefault();
        }

        public IQueryable<Product> GetProducts()
        {
            return FindAll().OrderByDescending(p => p.Id);
        }

        public ICollection<Product> GetProducts(QueryParameter parameter)
        {
            var products = FindAll();

            if (!string.IsNullOrEmpty(parameter.ProductName))
            {
                products = products.Where(p => !string.IsNullOrEmpty(p.Name) && 
                                                p.Name.ToLower().Contains(parameter.ProductName.ToLower()));
            }

            if (parameter.BrandId.HasValue)
            {
                products = products.Where(p => p.BrandId == parameter.BrandId);
            }

            if (parameter.CategoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == parameter.CategoryId);
            }

            if (string.IsNullOrEmpty(parameter.OrderType))
            {
                products = products.OrderByDescending(p => p.Id);
            }
            else
            {
                if (parameter.OrderType == "NAME ASC")
                {
                    products = products.OrderBy(p => p.Name);
                }
                else if (parameter.OrderType == "NAME DESC")
                {
                    products = products.OrderByDescending(p => p.Name);
                }
                else if (parameter.OrderType == "PRICE ASC")
                {
                    products = products.OrderBy(p => p.Price);
                }
                else if (parameter.OrderType == "PRICE DESC")
                {
                    products = products.OrderByDescending(p => p.Price);
                }
            }            

            CollectActivities(parameter);

            products = products.Include(p => p.Brand)
                                .Include(p => p.Category);

            return GetItemsByPage(products, parameter.PageNumber, parameter.PageSize).ToList();
        }        

        public void CreateProduct(Product product, string userId)
        {
            product.CreatedAt = DateTime.Now;
            product.CreatedBy = userId;
            Add(product);
            EventLog(EventType.Create, string.Empty, JsonSerializer.Serialize(product), userId);
            context.SaveChanges();
        }

        public void DeleteProduct(int id, string userId)
        {
            var product = FindByCondition(p => p.Id == id).SingleOrDefault();
            if (product != null)
            {
                Delete(product);
                EventLog(EventType.Delete, JsonSerializer.Serialize(product), string.Empty, userId);
                context.SaveChanges();
            }            
        }

        public void UpdateProduct(Product product, string userId)
        {
            product.ModifiedAt = DateTime.Now;
            product.ModifiedBy = userId;
            var currentProduct = GetProduct(product.Id);
            Update(product);
            EventLog(EventType.Update, JsonSerializer.Serialize(currentProduct), JsonSerializer.Serialize(product), userId);
            context.SaveChanges();
        }

        public PageInfo<Product> GetProductPageInfo(QueryParameter parameter)
        {
            return GetPageInfo(parameter);
        }

        private void CollectActivities(QueryParameter parameter)
        {
            var searchActivity = new Activity();

            if (!string.IsNullOrEmpty(parameter.ProductName))
            {
                searchActivity.SearchType = ProductProperty.Name; 
                searchActivity.SearchValue = parameter.ProductName;
            }

            if (parameter.BrandId.HasValue)
            {
                searchActivity.FilterType = ProductProperty.Brand;
                searchActivity.FilterValue = parameter.BrandId;
            }

            if (parameter.CategoryId.HasValue)
            {
                searchActivity.FilterType = ProductProperty.Category;
                searchActivity.FilterValue = parameter.CategoryId;
            }

            if (string.IsNullOrEmpty(parameter.OrderType))
            {
                searchActivity.OrderType = OrderType.Descending;
                searchActivity.OrderValue = ProductProperty.Id;
            }
            else
            {
                searchActivity.OrderType = parameter.OrderType.Contains("ASC", StringComparison.InvariantCultureIgnoreCase) ? OrderType.Ascending : OrderType.Descending;
                searchActivity.OrderValue = parameter.OrderType.Contains("NAME", StringComparison.InvariantCultureIgnoreCase) ? ProductProperty.Name : ProductProperty.Price;
            }            
            EventLog(EventType.Create, string.Empty, JsonSerializer.Serialize(searchActivity), string.Empty);
            activity.CreateActivity(searchActivity);
        }

        private void CollectActivities(int id)
        {
            var searchActivity = new Activity 
            { 
                SearchType = ProductProperty.Id, 
                SearchValue = Convert.ToString(id)
            };
            EventLog(EventType.Create, string.Empty, JsonSerializer.Serialize(searchActivity), string.Empty);
            activity.CreateActivity(searchActivity);
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