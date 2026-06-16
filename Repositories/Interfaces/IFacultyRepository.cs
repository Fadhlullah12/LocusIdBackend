using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IFacultyRepository : IBaseRepository<Faculty>
    {
        Task<Faculty> Get(string id);
        Task<Faculty> Get(Expression<Func<Faculty, bool>> expression);
        Task<ICollection<Faculty>> GetFacultysByIds(ICollection<string> FacultyIds);
        Task<ICollection<Faculty>> GetFaculties(Expression<Func<Faculty, bool>> expression);

    }
}