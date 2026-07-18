using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IGeoService _geoService;
        private readonly IAcademicSessionRepository _academicSessionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentSessionRepository _studentSessionRepository; 
        private readonly IDecodeTokenService _decodeTokenService;
        public AttendanceService(ISessionRepository sessionRepository, IStudentRepository studentRepository, IAttendanceRepository attendanceRepository, IGeoService geoService, IAcademicSessionRepository academicSessionRepository, 
                                ICourseRepository courseRepository, IDecodeTokenService decodeTokenService, IStudentSessionRepository studentSessionRepository)
        {
            _sessionRepository = sessionRepository;
            _studentRepository = studentRepository;
            _attendanceRepository = attendanceRepository;
            _geoService = geoService;
            _academicSessionRepository = academicSessionRepository;
            _courseRepository = courseRepository;
            _decodeTokenService = decodeTokenService;
            _studentSessionRepository = studentSessionRepository;
        }

        public async Task<BaseResponse<int>> CheckAttendanceEligibility(CheckAttendanceEligibilityRequestModel model)
        {
            string userId = _decodeTokenService.GetIdFromRawToken(model.Token);
            var student = await _studentRepository.Get(s => s.UserId == userId);
            var academicSession = await _academicSessionRepository.GetAcademicSession(s => s.IsActive);
            if (academicSession == null)
            {
                return new BaseResponse<int>
                {
                    Success = false,
                    Message = "No active academic session found"
                };
            }
            var course = await _courseRepository.GetById(model.CourseId);
            if (course == null)
            {
                return new BaseResponse<int>
                {
                    Success = false,
                    Message = "Course not found"
                };
            }
            var attendances = course.Sessions.Where(a => a.AcademicSessionId == academicSession.Id).SelectMany(s => s.Attendances).Where(a => a.StudentId == student.Id).Count();
            int heldSessions = course.Sessions.Count();
            if (heldSessions == 0)
            {
                return new BaseResponse<int>
                {
                    Success = false,
                    Message = "No sessions held for this course yet"
                };
            }
            var attendancePercentage = attendances / (double)heldSessions * 100;
            if (attendancePercentage < 75)
            {
                return new BaseResponse<int>
                {
                    Success = false,
                    Message = $"Attendance percentage is {attendancePercentage}%. Minimum required is 75%",
                    Data = (int)attendancePercentage
                };
            }
            return new BaseResponse<int>
            {
                Success = true,
                Message = "Attendance percentage is sufficient",
                Data = (int)attendancePercentage
            };
          
        }

        public async Task<BaseResponse<AttendanceDto>> MarkAttendance(MarkAttendanceRequestModel request, string token)
        {
            var course = await _courseRepository.Get(c => c.CourseCode == request.CourseCode);
            var session = await _sessionRepository.Get(s => s.CourseId == course.Id && s.IsActive);
            var userId = _decodeTokenService.GetIdFromRawToken(token);
            var student = await _studentRepository.Get( s => s.UserId == userId);
            if (session == null)
            {
                return new BaseResponse<AttendanceDto>
                {
                    Success = false,
                    Message = "Sorry this Session has ended or No session has been started for this course yet"
                };
            }
            var attendanceExists = await _attendanceRepository.Get(a => a.SessionId == session.Id && a.StudentId == student.Id);
            if (attendanceExists != null)
            {
                return new BaseResponse<AttendanceDto>
                {
                    Success = false,
                    Message = "Attendance already marked for this session"
                };
            }
             var distance = _geoService.IsWithinRange(request.Latitude, request.Longitude, session.Latitude, session.Longitude, 1);
            if (!distance)
            {
                return new BaseResponse<AttendanceDto>
                {
                    Success = false,
                    Message = "You are not within the required range to mark attendance"
                };
            }
            var attendance = new Attendance
            {
                StudentId = student.Id,
                SessionId = session.Id,
                Session = session,
                Student = student,
                Attended = true,
            };

             var studentSession = new StudentSession
            {
                StudentId = student.Id,
                SessionId = session.Id,
                Session = session,
                Student = student,
                Status = true,
            };
            session.Attendances.Add(attendance);
            student.Attendances.Add(attendance);
            student.StudentSessions.Add(studentSession);
            session.StudentSessions.Add(studentSession);
            await _studentSessionRepository.Create(studentSession);
            await _attendanceRepository.Create(attendance);
            await _sessionRepository.Save();
            return new BaseResponse<AttendanceDto>
            {
                Success = true,
                Message = "Attendance marked successfully",
                Data = new AttendanceDto
                {
                    StudentId = student.Id,
                    SessionId = session.Id,
                    StudentName = $"{student.User.FirstName} {student.User.LastName}",
                    CourseTitle = session.Course.CourseName
                }
            };
            
        }
    }
}