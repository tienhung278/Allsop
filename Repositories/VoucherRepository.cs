using Allsop.Contracts;
using Allsop.Models;
using Allsop.Models.Entities;
using System.Text.Json;

namespace Allsop.Repositories
{
    public class VoucherRepository : RepositoryBase<Voucher>, IVoucherRepository
    {
        private readonly RepositoryContext context;
        private readonly IEventStoreRepository eventStore;

        public VoucherRepository(RepositoryContext context,
                                IEventStoreRepository eventStore) : base(context)
        {
            this.context = context;
            this.eventStore = eventStore;
        }        

        public Voucher? GetVoucher(int id)
        {
            return FindByCondition(p => p.Id == id).SingleOrDefault();
        }

        public IQueryable<Voucher> GetVouchers()
        {
            return FindAll().OrderByDescending(b => b.Id);
        }

        public ICollection<Voucher> GetVouchers(QueryParameter parameter)
        {
            return GetItemsByPage(FindAll().OrderByDescending(b => b.Id), parameter.PageNumber, parameter.PageSize).ToList();
        }        

        public void CreateVoucher(Voucher voucher, string userId)
        {
            voucher.CreatedAt = DateTime.Now;
            voucher.CreatedBy = userId;
            Add(voucher);
            EventLog(EventType.Create, string.Empty, JsonSerializer.Serialize(voucher), userId);
            context.SaveChanges();
        }

        public void DeleteVoucher(Voucher voucher, string userId)
        {
            Delete(voucher);
            EventLog(EventType.Delete, JsonSerializer.Serialize(voucher), string.Empty, userId);
            context.SaveChanges();
        }

        public void UpdateVoucher(Voucher voucher, string userId)
        {
            voucher.ModifiedAt = DateTime.Now;
            voucher.ModifiedBy = userId;
            var currentVoucher = GetVoucher(voucher.Id);
            Update(voucher);
            EventLog(EventType.Update, JsonSerializer.Serialize(currentVoucher), JsonSerializer.Serialize(voucher), userId);
            context.SaveChanges();
        }

        public PageInfo<Voucher> GetVoucherPageInfo(QueryParameter parameter)
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