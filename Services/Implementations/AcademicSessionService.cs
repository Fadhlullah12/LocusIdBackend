using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class AcademicSessionService : IAcademicSessionService
    {
        private readonly IAcademicSessionRepository _academicSessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDecodeTokenService _decodeTokenService;
        private readonly IDirectorRepository _directorRepository;

        public AcademicSessionService(IAcademicSessionRepository academicSessionRepository, IUserRepository userRepository, IDecodeTokenService decodeTokenService, IDirectorRepository directorRepository)
        {
            _academicSessionRepository = academicSessionRepository;
            _userRepository = userRepository;
            _decodeTokenService = decodeTokenService;
            _directorRepository = directorRepository;
        }

        public async Task<BaseResponse<AcademicSessionDto>> CreateAcademicSession(CreateAcademicSessionRequestModel request, string token)
        {
            string userId = _decodeTokenService.GetIdFromRawToken(token);
            var director = await _directorRepository.Get(d => d.UserId == userId);

            var existingAcademicSession = await _academicSessionRepository.GetAcademicSession(a => a.IsActive);
            if (existingAcademicSession != null)
            {
                return new BaseResponse<AcademicSessionDto>
                {
                    Message = $"AcademicSession {existingAcademicSession.Name} is still ongoing",
                    Success = false
                };
            }

            var academicSession = new AcademicSession
            {
                Name = request.Name,
                StartDate = DateTime.Now.Date,
                School = director.School,
                SchoolId = director.School.Id,
            };

            await _academicSessionRepository.Create(academicSession);
            await _academicSessionRepository.Save();

            return new BaseResponse<AcademicSessionDto>
            {
                Message = $"Academic Session {academicSession.Name} started",
                Success = true,
                Data = new AcademicSessionDto
                {
                    Name = academicSession.Name,
                    StartDate = academicSession.StartDate,
                    IsActive = academicSession.IsActive,
                }
            };
        }

        public async Task<BaseResponse<string>> DeleteAcademicSession(string academicSessionId, string token)
        {
            var userId = _decodeTokenService.GetIdFromRawToken(token);
            if (userId == null) return new BaseResponse<string> { Success = false, Message = "Invalid token" };

            var director = await _directorRepository.Get(d => d.UserId == userId);
            if (director == null) return new BaseResponse<string> { Success = false, Message = "You are not authorized" };

            var academicSession = await _academicSessionRepository.GetAcademicSessionById(academicSessionId);
            if (academicSession == null) return new BaseResponse<string> { Success = false, Message = "Academic session not found" };

            var deleted = await _academicSessionRepository.Delete(academicSession.Id);
            if (!deleted) return new BaseResponse<string> { Success = false, Message = "Failed to delete academic session" };

            await _academicSessionRepository.Save();
            return new BaseResponse<string> { Success = true, Message = "Academic session deleted successfully" };
        }
    }
}