using Allsop.Models;
using Allsop.Models.Entities;

namespace Allsop.Contracts
{
    public interface IOrderDetailRepository
    {
        ICollection<OrderDetail> GetOrderDetails(QueryParameter parameter);
        OrderDetail? GetOrderDetail(int id);        
        void CreateOrderDetail(OrderDetail orderDetail, string userId);
        void UpdateOrderDetail(OrderDetail orderDetail, string userId);
        void DeleteOrderDetail(OrderDetail orderDetail, string userId);
        PageInfo<OrderDetail> GetOrderDetailPageInfo(QueryParameter parameter);
    }
}