using Allsop.Models;
using Allsop.Models.Entities;

namespace Allsop.Contracts
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetCategories();
        ICollection<Category> GetCategories(QueryParameter parameter);
        Category? GetCategory(int id);        
        void CreateCategory(Category category, string userId);
        void UpdateCategory(Category category, string userId);
        void DeleteCategory(Category category, string userId);
        PageInfo<Category> GetCategoryPageInfo(QueryParameter parameter);
    }
}