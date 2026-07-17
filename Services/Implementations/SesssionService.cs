using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IDecodeTokenService _decodeTokenService;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IAcademicSessionRepository _academicSessionRepository;


        public SessionService(ISessionRepository sessionRepository, ICourseRepository courseRepository, IStudentRepository studentRepository, 
                              IDecodeTokenService decodeTokenService, ILecturerRepository lecturerRepository
                              ,IAcademicSessionRepository academicSessionRepository
                            )
        {
            _sessionRepository = sessionRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _decodeTokenService = decodeTokenService;
            _lecturerRepository = lecturerRepository;
            _academicSessionRepository = academicSessionRepository;
        }

        public async Task<BaseResponse<SessionDto>> CreateSession(CreateSessionRequestModel request, string token)
        {
            var course = await _courseRepository.Get(c => c.CourseCode == request.CourseCode);
            if (course == null)
            {
                return new BaseResponse<SessionDto> { Success = false, Message = "Course not found" };
            }

            var academicSession = await _academicSessionRepository.GetAcademicSession(s => s.IsActive);
            if (academicSession == null)
            {
                return new BaseResponse<SessionDto> { Success = false, Message = "No active academic session found" };
            }

            string userId = _decodeTokenService.GetIdFromRawToken(token);
            if (userId == null)
            {
                return new BaseResponse<SessionDto> { Success = false, Message = "Invalid token" };
            }

            var lecturer = await _lecturerRepository.Get(u => u.UserId == userId);
            if (lecturer == null || course.LecturerId != lecturer.Id)
            {
                return new BaseResponse<SessionDto> { Success = false, Message = "You are not authorized to start a session for this course" };
            }

            var activeSessionExists = await _sessionRepository.Get(s => s.CourseId == course.Id && s.IsActive);
            if (activeSessionExists != null)
            {
                return new BaseResponse<SessionDto> { Success = false, Message = "An active session already exists for this course" };
            }

           var session = new Session
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                CourseId = course.Id,
                LecturerId = lecturer.Id,
                Duration = request.Duration,
                AcademicSessionId = academicSession.Id,
                IsActive = true, // Ensure it starts as active
                CreatedAt = DateTime.Now, 
            };            
            lecturer.Sessions.Add(session);
            await _sessionRepository.Create(session);
            await _sessionRepository.Save();
            return new BaseResponse<SessionDto>
            {
                Success = true,
                Message = "Session started successfully",
                Data = new SessionDto
                {
                    Id = session.Id,
                    IsActive = session.IsActive,
                    CourseName = course.CourseName,
                    LecturerName = $"{lecturer.User?.FirstName} {lecturer.User?.LastName}"
                }
            };
        }
        public async Task<BaseResponse<SessionStudentDto>> GetSessionStudents(string sessionId)
        {
            var session = await _sessionRepository.Get(s => s.Id == sessionId);
            if (session == null)
            {
                return new BaseResponse<SessionStudentDto>
                {
                    Success = false,
                    Message = "Session not found"
                };
            }
            var students = await _studentRepository.GetAll(s => s.StudentSessions.Any(ss => ss.SessionId == sessionId));
            var studentDtos = students.Select(s => new StudentDto
            {
                Id = s.Id,
                FullName = $"{s.User.FirstName} {s.User.LastName}",                 
                MatricNumber = s.MatricNumber,
            }).ToList();
            return new BaseResponse<SessionStudentDto>
            {
                Success = true,
                Message = "Students retrieved successfully",
                Data = new SessionStudentDto
                {
                    SessionId = session.Id,
                    Students = studentDtos
                }
            };

        }

    
    }
}