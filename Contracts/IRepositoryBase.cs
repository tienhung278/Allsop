using Allsop.Models;
using Allsop.Models.Entities;
using System.Linq.Expressions;

namespace Allsop.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expresstion);
        IQueryable<T> GetItemsByPage(IQueryable<T> collection, int pageNumber, int pageSize);
        PageInfo<T> GetPageInfo(QueryParameter parameter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}