using Allsop.Models;
using Allsop.Models.Entities;

namespace Allsop.Contracts
{
    public interface IBrandRepository
    {
        IQueryable<Brand> GetBrands();
        ICollection<Brand> GetBrands(QueryParameter parameter);
        Brand? GetBrand(int id);        
        void CreateBrand(Brand brand, string userId);
        void UpdateBrand(Brand brand, string userId);
        void DeleteBrand(Brand brand, string userId);
        PageInfo<Brand> GetBrandPageInfo(QueryParameter parameter);
    }
}