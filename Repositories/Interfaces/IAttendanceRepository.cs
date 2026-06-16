using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IAttendanceRepository : IBaseRepository<Attendance>
    {
        Task<Attendance?> Get(Expression<Func<Attendance, bool>> expression);
    }
}
