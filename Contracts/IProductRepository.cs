using Allsop.Models;
using Allsop.Models.DTOs;
using Allsop.Models.Entities;

namespace Allsop.Contracts
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
        ICollection<Product> GetProducts(QueryParameter parameter);
        Product? GetProduct(int id);        
        void CreateProduct(Product product, string userId);
        void UpdateProduct(Product product, string userId);
        void DeleteProduct(int id, string userId);
        PageInfo<Product> GetProductPageInfo(QueryParameter parameter);
    }
}