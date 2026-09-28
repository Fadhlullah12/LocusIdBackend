using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDecodeTokenService _decodeTokenService;
        private readonly IUserRepository _userRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly IAcademicSessionRepository _academicSessionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentSessionRepository _studentSessionRepository;

        public CourseService(ICourseRepository courseRepository, IDecodeTokenService decodeTokenService, IUserRepository userRepository,
                             IStudentCourseRepository studentCourseRepository, IAcademicSessionRepository academicSessionRepository, IStudentRepository studentRepository,
                             ISessionRepository sessionRepository, IAttendanceRepository attendanceRepository, IStudentSessionRepository studentSessionRepository)
        {
            _courseRepository = courseRepository;
            _decodeTokenService = decodeTokenService;
            _userRepository = userRepository;
            _studentCourseRepository = studentCourseRepository;
            _academicSessionRepository = academicSessionRepository;
            _studentRepository = studentRepository;
            _sessionRepository = sessionRepository;
            _attendanceRepository = attendanceRepository;
            _studentSessionRepository = studentSessionRepository;
        }

        public async Task<BaseResponse<CourseDto>> CreateCourse(CreateCourseRequestModel model,string token)
        {
             var accademicSession = await _academicSessionRepository.GetAcademicSession(s => s.IsActive);
            if(accademicSession == null)
            {
                return new BaseResponse<CourseDto>
                {
                    Success = false,
                    Message = "No active academic session found"
                };
            }

            if( DateTime.UtcNow.Month - accademicSession.StartDate.Month > 1)
            {
                return new BaseResponse<CourseDto>
                {
                    Success = false,
                    Message = "Cannot delete course"
                };
            }
            var existingCourse = await _courseRepository.Get(c => c.CourseCode == model.CourseCode);
            string userId = _decodeTokenService.GetIdFromRawToken(token);
            if(userId == null)
            {
                return new BaseResponse<CourseDto>
                {
                    Success = false,
                    Message = "Invalid token"
                };
            }
            var user = await _userRepository.Get(u => u.Id == userId);
            if (existingCourse != null)
                {
                BaseResponse<CourseDto> response = new BaseResponse<CourseDto>
                {
                    Success = false,
                    Message = "Course already exists"
                };
                return response;
            }
            var course = new Course
            {
               CourseCode = model.CourseCode,
               CourseName = model.CourseName,
                LecturerId = user.Lecturer.Id,
                Lecturer = user.Lecturer,
            };
            user.Lecturer.Courses.Add(course);
            await _courseRepository.Create(course);
            await _courseRepository.Save();
            return new BaseResponse<CourseDto>
            {
                Success = true,
                Message = "Cousre Created Sucessfully",
                Data = new CourseDto
                {
                    CourseCode = course.CourseCode,
                    CourseName = course.CourseName,
                }
            };
        }

            public async Task<BaseResponse<ICollection<SessionDto>>> CourseSessions(string courseCode)
        {
            // Ensure the repository includes the Sessions navigation property
            var course = await _courseRepository.Get(c => c.CourseCode == courseCode);

            if (course == null)
            {
                return new BaseResponse<ICollection<SessionDto>>
                {
                    Success = false,
                    Message = "Course not found"
                };
            }

            // Defensive check in case Sessions is null
            var sessions = course.Sessions?.Select(s => new SessionDto
            {
                Id = s.Id,
                StartTime = s.CreatedAt.ToString("yyyy-MM-dd"),
                DurationMinutes = (s.EndTime - s.CreatedAt)?.ToString(@"hh\:mm\:ss") ?? "00:00:00",
                IsActive = s.IsActive,
            }).ToList();

            return new BaseResponse<ICollection<SessionDto>>
            {
                Success = true,
                Message = "Sessions retrieved successfully",
                Data = sessions
            };
        }
         public async Task<BaseResponse<OverallCourseAttendance>> CourseAttendance(string courseCode,string token)
        {
            var userID = _decodeTokenService.GetIdFromRawToken(token);
            var student = await _studentRepository.Get(u => u.UserId == userID);
            var course = await _courseRepository.Get(c => c.CourseCode == courseCode);
            if(course == null)
            {
                return new BaseResponse<OverallCourseAttendance>
                {
                    Success = false,
                    Message = "Course not found"
                };
            }
             var attendancePercentage = await CheckAttendanceEligibility(student.Id, course.Id);
            var sessions = course.Sessions.Select(s => s.Attendances);
            var courseAttendanceDtos = new OverallCourseAttendance
            {
                AttendancePercentage = attendancePercentage,
                Attendances = sessions.SelectMany(a => a).Where(a => a.StudentId == student.Id).Select(a => new CourseAttendanceDto
                {
                    DateCreated = a.Session.CreatedAt.ToString("yyyy-MM-dd"),
                    Status = a.Attended,
                    TimeMarked = a.CreatedAt.ToString("HH:mm:ss"),
                }).ToList(),
            };
                int attendanceCount = courseAttendanceDtos.Attendances.Where(a => a.Status).Count();
                courseAttendanceDtos.AttendedClasses = attendanceCount;
           
            return new BaseResponse<OverallCourseAttendance>
            {
                Success = true,
                Message = "Attendance retrieved successfully",
                Data = courseAttendanceDtos
            };
        }

        public async Task<BaseResponse<ICollection<StudentDto>>> GetCourseStudents(string courseCode)
        {
            var course = await _courseRepository.Get(c => c.CourseCode == courseCode);
            if (course == null)
            {
                return new BaseResponse<ICollection<StudentDto>>
                {
                    Success = false,
                    Message = "Course not found"
                };
            }

            var studentEnrollments = await _studentCourseRepository.GetAll(a => a.CourseId == course.Id);

            // 1. Initialize the list to hold our DTOs
            var studentDtos = new List<StudentDto>();

            // 2. Use a foreach loop instead of .Select(async...)
            // This processes students one by one, preventing concurrent DbContext usage.
            foreach (var s in studentEnrollments)
            {
                var attendanceResult = await CheckAttendanceEligibility(s.Student.Id, course.Id);

                studentDtos.Add(new StudentDto
                {
                    Id = s.Student.Id,
                    FullName = $"{s.Student.User.FirstName} {s.Student.User.LastName}",
                    MatricNumber = s.Student.MatricNumber,
                    Department = s.Student.Department.Name,
                    Faculty = s.Student.Faculty.Name,
                    AttendancePercentage = attendanceResult
                });
            }

            return new BaseResponse<ICollection<StudentDto>>
            {
                Success = true,
                Message = "Students retrieved successfully",
                Data = studentDtos 
            };
        }
        private async Task<int> CheckAttendanceEligibility(string studentId, string courseId)
        {
            var academicSession = await _academicSessionRepository.GetAcademicSession(s => s.IsActive);
            if (academicSession == null)
            {
               return 0;
            }
            var course = await _courseRepository.GetById(courseId);
            if (course == null)
            {
                return 0;
            }
              
            
            var attendances = course.Sessions.Where(a => a.AcademicSessionId == academicSession.Id).SelectMany(s => s.Attendances).Where(a => a.StudentId == studentId && a.Attended == true).Count();
            int heldSessions = course.Sessions.Count();
            if (heldSessions == 0)
            {
                return 0;
            }
            var attendancePercentage = attendances / (double)heldSessions * 100;
            return (int)attendancePercentage;    
        }

        public async Task<BaseResponse<ICollection<CourseDto>>> GetCourses()
        {
           var courses = await _courseRepository.GetAll();
            var courseDtos = courses.Select(c => new CourseDto
            {
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
            }).ToList();
            return new BaseResponse<ICollection<CourseDto>>
            {
                Success = true,
                Message = "Courses retrieved successfully",
                Data = courseDtos
            };
        }

        public async Task<BaseResponse<string>> DeleteCourse(string courseCode, string token)
        {
            var accademicSession = await _academicSessionRepository.GetAcademicSession(s => s.IsActive);
            if(accademicSession == null)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "No active academic session found"
                };
            }

            if( DateTime.UtcNow.Month - accademicSession.StartDate.Month > 1)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Cannot delete course"
                };
            }
            var userId = _decodeTokenService.GetIdFromRawToken(token);
            if (userId == null)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Invalid token"
                };
            }

            var user = await _userRepository.Get(u => u.Id == userId);
            if (user == null)
            {
                return new BaseResponse<string> { Success = false, Message = "User not found" };
            }

            var course = await _courseRepository.Get(c => c.CourseCode == courseCode);
            if (course == null)
            {
                return new BaseResponse<string> { Success = false, Message = "Course not found" };
            }

            if (user.Role != "Director")
            {
                if (user.Lecturer == null || user.Lecturer.Id != course.LecturerId)
                {
                    return new BaseResponse<string> { Success = false, Message = "You are not authorized to delete this course" };
                }
            }

            // Remove enrollments
            if (course.CourseStudents != null && course.CourseStudents.Any())
            {
                _studentCourseRepository.DeleteRange(course.CourseStudents);
            }

            // Remove sessions and related attendance/student-session records
            if (course.Sessions != null && course.Sessions.Any())
            {
                foreach (var session in course.Sessions.ToList())
                {
                    if (session.Attendances != null && session.Attendances.Any())
                    {
                        _attendanceRepository.DeleteRange(session.Attendances);
                    }
                    if (session.StudentSessions != null && session.StudentSessions.Any())
                    {
                        _studentSessionRepository.DeleteRange(session.StudentSessions);
                    }
                    await _sessionRepository.Delete(session.Id);
                }
            }

            var deleted = await _courseRepository.Delete(course.Id);
            if (!deleted)
            {
                return new BaseResponse<string> { Success = false, Message = "Failed to delete course" };
            }

            await _courseRepository.Save();

            return new BaseResponse<string> { Success = true, Message = "Course deleted successfully" };
        }
    }
}