using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface ISchoolRepository : IBaseRepository<School>
    {
        Task<School> Get(string id);
        Task<School> Get(Expression<Func<School, bool>> expression);
        Task<ICollection<School>> GetSchoolsByIds(ICollection<string> SchoolIds);
        Task<ICollection<School>> GetSchools(Expression<Func<School, bool>> expression);
    }
}