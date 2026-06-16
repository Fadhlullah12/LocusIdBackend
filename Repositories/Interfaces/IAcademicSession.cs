using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IAcademicSessionRepository : IBaseRepository<AcademicSession>
    {
        Task<ICollection<AcademicSession>> GetAcademicSessions(Expression<Func<AcademicSession, bool>> expression);
        Task<AcademicSession> GetAcademicSessionById(string Id);
        Task<AcademicSession> GetAcademicSession(Expression<Func<AcademicSession, bool>> expression);
    }
}