using Allsop.Contracts;
using Allsop.Models;
using Allsop.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Allsop.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly RepositoryContext context;
        private readonly IVoucherRepository voucher;
        private readonly IEventStoreRepository eventStore;

        public OrderRepository(RepositoryContext context,
                                IVoucherRepository voucher,
                                IEventStoreRepository eventStore) : base(context)
        {
            this.context = context;
            this.eventStore = eventStore;
            this.voucher = voucher;
        }        

        public Order? GetOrder(int id)
        {
            return FindByCondition(p => p.Id == id)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .SingleOrDefault();
        }

        public IQueryable<Order> GetOrders()
        {
            return FindAll()
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .OrderByDescending(b => b.Id);
        }

        public ICollection<Order> GetOrders(QueryParameter parameter)
        {
            return GetItemsByPage(FindAll()
                                    .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Product)
                                    .OrderByDescending(b => b.Id), parameter.PageNumber, parameter.PageSize).ToList();
        }        

        public bool CreateOrder(Order order, string userId)
        {
            if (!string.IsNullOrEmpty(order.VoucherCode) && !IsValidVoucher(order.VoucherCode))
            {
                return false;
            }
            order.CreatedAt = DateTime.Now;
            order.CreatedBy = userId;            
            Add(order);
            EventLog(EventType.Create, string.Empty, JsonSerializer.Serialize(order), userId);
            context.SaveChanges();
            return true;
        }

        public void DeleteOrder(Order order, string userId)
        {
            Delete(order);
            EventLog(EventType.Delete, JsonSerializer.Serialize(order), string.Empty, userId);
            context.SaveChanges();
        }

        public bool UpdateOrder(Order order, string userId)
        {
            if (!string.IsNullOrEmpty(order.VoucherCode) && !IsValidVoucher(order.VoucherCode))
            {
                return false;
            }
            order.ModifiedAt = DateTime.Now;
            order.ModifiedBy = userId;
            var currentOrder = GetOrder(order.Id);
            Update(order);
            EventLog(EventType.Update, JsonSerializer.Serialize(currentOrder), JsonSerializer.Serialize(order), userId);
            context.SaveChanges();
            return true;
        }

        public PageInfo<Order> GetOrderPageInfo(QueryParameter parameter)
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

        private bool IsValidVoucher(string code)
        {
            return voucher.GetVouchers().Any(v => !string.IsNullOrEmpty(v.Code) && 
                                                    v.Code.ToLower() == code.ToLower());
        }
    }
}