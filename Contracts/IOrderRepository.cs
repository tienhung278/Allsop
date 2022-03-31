using Allsop.Models;
using Allsop.Models.Entities;

namespace Allsop.Contracts
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetOrders();
        ICollection<Order> GetOrders(QueryParameter parameter);
        Order? GetOrder(int id);        
        bool CreateOrder(Order order, string userId);
        bool UpdateOrder(Order order, string userId);
        void DeleteOrder(Order order, string userId);
        PageInfo<Order> GetOrderPageInfo(QueryParameter parameter);
    }
}