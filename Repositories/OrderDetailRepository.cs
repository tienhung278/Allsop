using Allsop.Contracts;
using Allsop.Models;
using Allsop.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Allsop.Repositories
{
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        private readonly RepositoryContext context;
        private readonly IEventStoreRepository eventStore;
        private readonly IProductRepository product;

        public OrderDetailRepository(RepositoryContext context,
                                IProductRepository product,
                                IEventStoreRepository eventStore) : base(context)
        {
            this.context = context;
            this.eventStore = eventStore;
            this.product = product;
        }        

        public OrderDetail? GetOrderDetail(int id)
        {
            return FindByCondition(p => p.Id == id)
                    .Include(od => od.Product)
                    .SingleOrDefault();
        }

        public ICollection<OrderDetail> GetOrderDetails(QueryParameter parameter)
        {
            return GetItemsByPage(FindAll()
                                    .Include(od => od.Product)
                                    .OrderByDescending(b => b.Id), parameter.PageNumber, parameter.PageSize).ToList();
        }        

        public void CreateOrderDetail(OrderDetail orderDetail, string userId)
        {
            if (IsValidProduct(orderDetail))
            {
                orderDetail.CreatedAt = DateTime.Now;
                orderDetail.CreatedBy = userId;
                Add(orderDetail);
                EventLog(EventType.Create, string.Empty, JsonSerializer.Serialize(orderDetail), userId);
                context.SaveChanges();
            }            
        }

        public void DeleteOrderDetail(OrderDetail orderDetail, string userId)
        {
            Delete(orderDetail);
            EventLog(EventType.Delete, JsonSerializer.Serialize(orderDetail), string.Empty, userId);
            context.SaveChanges();
        }

        public void UpdateOrderDetail(OrderDetail orderDetail, string userId)
        {
            if (IsValidProduct(orderDetail))
            {
                orderDetail.ModifiedAt = DateTime.Now;
                orderDetail.ModifiedBy = userId;
                var currentOrderDetail = GetOrderDetail(orderDetail.Id);
                Update(orderDetail);
                EventLog(EventType.Update, JsonSerializer.Serialize(currentOrderDetail), JsonSerializer.Serialize(orderDetail), userId);
                context.SaveChanges();
            }            
        }

        public PageInfo<OrderDetail> GetOrderDetailPageInfo(QueryParameter parameter)
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

        private bool IsValidProduct(OrderDetail orderDetail)
        {
            int quantity = 0;

            var p = product.GetProducts()
                            .SingleOrDefault(p => p.Id == orderDetail.ProductId);
            if (p != null)
            {
                quantity = p.Quantity;
            }
            return orderDetail.Quantity <= quantity;
        }
    }    
}