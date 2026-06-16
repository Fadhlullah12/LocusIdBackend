using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IDirectorRepository : IBaseRepository<Director>
    {
        Task<Director> Get(string id);
        Task<Director> Get(Expression<Func<Director, bool>> expression);
        Task<ICollection<Director>> GetDepartmentsByIds(ICollection<string> departmentIds);
        Task<ICollection<Director>> GetDepartmentsByIds(Expression<Func<Director, bool>> expression);
    }
}