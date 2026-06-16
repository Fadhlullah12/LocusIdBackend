using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Student> Get(Expression<Func<Student, bool>> expression);
        Task<ICollection<Student>> GetAll(Expression<Func<Student, bool>> expression);
        Task<Student> GetId(string Id);
    }
}
