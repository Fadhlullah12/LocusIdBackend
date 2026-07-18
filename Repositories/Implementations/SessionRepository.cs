using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using System.Linq.Expressions;

namespace LocusIDBackend.Repositories.Implementations
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<Session> Get(Expression<Func<Session, bool>> expression)
        {
            var session = await _context.Sessions.Include(s => s.Course)
            .FirstOrDefaultAsync(expression);
            return session!;
        }

        public async Task<ICollection<Session>> GetAll(Expression<Func<Session, bool>> expression)
        {
             var sessions = await _context.Set<Session>()
            .Include(s => s.Course)
            .Where(expression)
            .ToListAsync();
            return sessions;
        }

        public async Task<Session> GetId(string Id)
        {
            var session = await _context.Sessions
            .Include(s => s.Course)
            .FirstOrDefaultAsync(s => s.Id == Id);
            return session!;
        }
    }
}
