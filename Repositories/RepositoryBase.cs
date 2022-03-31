using Microsoft.EntityFrameworkCore;
using Allsop.Contracts;
using Allsop.Models;
using Allsop.Models.Entities;
using System.Linq.Expressions;

namespace Allsop.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly RepositoryContext context;
        private readonly DbSet<T> table;

        public RepositoryBase(RepositoryContext context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public void Add(T entity)
        {
            table.Add(entity);
        }

        public void Delete(T entity)
        {
            table.Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            return table.AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expresstion)
        {
            return table.Where(expresstion)
                        .AsNoTracking();
        }

        public IQueryable<T> GetItemsByPage(IQueryable<T> collection, int pageNumber, int pageSize)
        {
            return collection.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        public PageInfo<T> GetPageInfo(QueryParameter parameter)
        {
            var totalCount = FindAll().Count();
            return new PageInfo<T>(totalCount, parameter.PageNumber, parameter.PageSize);
        }

        public void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}