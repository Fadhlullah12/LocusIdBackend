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
        public AcademicSessionService(IAcademicSessionRepository academicSessionRepository)
        {
            _academicSessionRepository = academicSessionRepository;
        }

        public async Task<BaseResponse<AcademicSessionDto>> CreateAcademicSession(CreateAcademicSessionRequestModel request)
        {
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
            };
            await _academicSessionRepository.Create(academicSession);
            await _academicSessionRepository.Save();
            return new BaseResponse<AcademicSessionDto>
            {
                Message = $"Accademic Session {academicSession.Name} started",
                Success = true,
                Data = new AcademicSessionDto
                {
                   Name = academicSession.Name,
                   StartDate = academicSession.StartDate,
                   IsActive = academicSession.IsActive,
                }
            };
            
        }

    }
}