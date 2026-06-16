using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task<Course> Get(Expression<Func<Course, bool>> expression);
        Task<ICollection<Course>> GetCoursesByIds(ICollection<string> courseIds);

    }
}
