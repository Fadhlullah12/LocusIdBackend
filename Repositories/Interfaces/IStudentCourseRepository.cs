using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IStudentCourseRepository : IBaseRepository<StudentCourse>
    {
        Task<ICollection<StudentCourse>> GetAll(Expression<Func<StudentCourse, bool>> expression);
    }
}
