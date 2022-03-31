using Allsop.Contracts;
using Allsop.Models;
using Allsop.Models.Entities;

namespace Allsop.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        private readonly RepositoryContext context;
        
        public ActivityRepository(RepositoryContext context) : base(context)
        {
            this.context = context;
        }

        public void CreateActivity(Activity activity)
        {
            activity.CreatedAt = DateTime.Now;
            Add(activity);
            context.SaveChanges();
        }

        public ICollection<Activity> GetActivities(QueryParameter parameter)
        {
            return GetItemsByPage(FindAll().OrderByDescending(a => a.Id), parameter.PageNumber, parameter.PageSize).ToList();
        }

        public Activity? GetActivity(int id)
        {
            return FindByCondition(a => a.Id == id).SingleOrDefault();
        }

        public PageInfo<Activity> GetActivityPageInfo(QueryParameter parameter)
        {
            return GetPageInfo(parameter);
        }
    }
}