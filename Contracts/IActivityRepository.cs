using Allsop.Models;
using Allsop.Models.Entities;

namespace Allsop.Contracts
{
    public interface IActivityRepository
    {
        ICollection<Activity> GetActivities(QueryParameter parameter);
        Activity? GetActivity(int id);        
        void CreateActivity(Activity Activity);
        PageInfo<Activity> GetActivityPageInfo(QueryParameter parameter);
    }
}