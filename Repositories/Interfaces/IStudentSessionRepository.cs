using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IStudentSessionRepository : IBaseRepository<StudentSession>
    {
        Task<IEnumerable<StudentSession>> GetSessionsByStudent(string studentId);
        Task<IEnumerable<StudentSession>> GetStudentsBySession(string sessionId);
        Task<StudentSession> GetByStudentAndSession(string studentId, string sessionId);
    }
}
