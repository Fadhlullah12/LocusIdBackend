using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using System.Linq.Expressions;

namespace LocusIDBackend.Repositories.Implementations
{
    public class AttendanceRepository : BaseRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(ApplicationContext context) : base(context) { }

        public async Task<Attendance?> Get(Expression<Func<Attendance, bool>> expression)
        {
            var attendance = await _context.Attendances.
            Include(a => a.Student)
            .Include(a => a.Session)
            .FirstOrDefaultAsync(expression);
            return attendance;
        }
    }
}
