using System.Linq.Expressions;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocusIDBackend.Repositories.Implementations
{
    public class AcademicSessionRepository : BaseRepository<AcademicSession>, IAcademicSessionRepository
    {
        public AcademicSessionRepository(ApplicationContext context) : base(context) { }

        public async Task<AcademicSession> GetAcademicSessionById(string Id)
        {
            var academicSession = await _context.Set<AcademicSession>()
            .FirstOrDefaultAsync(a => a.Id == Id);
            return academicSession!;
        }

        public Task<AcademicSession> GetAcademicSession(Expression<Func<AcademicSession, bool>> expression)
        {
            var academicSession = _context.Set<AcademicSession>()
            .FirstOrDefaultAsync(expression);
            return academicSession!;
        }

        public async Task<ICollection<AcademicSession>> GetAcademicSessions(Expression<Func<AcademicSession, bool>> expression)
        {
            var academicSessions = await _context.Set<AcademicSession>()
            .Where(expression).ToListAsync();
            return academicSessions;
        }
     
    }
}