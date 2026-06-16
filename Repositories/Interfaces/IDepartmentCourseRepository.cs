using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IDepartmentCourseRepository : IBaseRepository<DepartmentCourse>
    {
        Task<DepartmentCourse> Get(string Id);
        Task<DepartmentCourse> Get(Expression<Func<DepartmentCourse, bool>> expression);
        Task<ICollection<DepartmentCourse>> GetCoursesByIds(ICollection<string> courseIds);
        Task<ICollection<DepartmentCourse>> GetCoursesByIds(Expression<Func<DepartmentCourse,bool>> expression);

    }
}