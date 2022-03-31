using AutoMapper;
using Allsop.Models.DTOs;
using Allsop.Models.Entities;

namespace Allsop
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductRead>();
            CreateMap<ProductWrite, Product>();            
            CreateMap<Brand, BrandRead>();
            CreateMap<BrandWrite, Brand>();
            CreateMap<Category, CategoryRead>();
            CreateMap<CategoryWrite, Category>();
            CreateMap<Voucher, VoucherRead>();
            CreateMap<VoucherWrite, Voucher>();
            CreateMap<Order, OrderRead>();
            CreateMap<OrderWrite, Order>();
            CreateMap<OrderDetail, OrderDetailRead>();
            CreateMap<OrderDetailWrite, OrderDetail>();
            CreateMap<Activity, ActivityRead>();
            CreateMap<EventStore, EventStoreRead>();            
        }
    }
}