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
        public CourseService(ICourseRepository courseRepository, IDecodeTokenService decodeTokenService, IUserRepository userRepository,
                             IStudentCourseRepository studentCourseRepository, IAcademicSessionRepository academicSessionRepository, IStudentRepository studentRepository)
        {
            _courseRepository = courseRepository;
            _decodeTokenService = decodeTokenService;
            _userRepository = userRepository;
            _studentCourseRepository = studentCourseRepository;
            _academicSessionRepository = academicSessionRepository;
            _studentRepository = studentRepository;
        }

        public async Task<BaseResponse<CourseDto>> CreateCourse(CreateCourseRequestModel model,string token)
        {
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
         public async Task<BaseResponse<ICollection<CourseAttendanceDto>>> CourseAttendance(string courseCode,string token)
        {
            var userID = _decodeTokenService.GetIdFromRawToken(token);
            var student = await _studentRepository.Get(u => u.UserId == userID);
            var course = await _courseRepository.Get(c => c.CourseCode == courseCode);
            if(course == null)
            {
                return new BaseResponse<ICollection<CourseAttendanceDto>>
                {
                    Success = false,
                    Message = "Course not found"
                };
            }
            var sessions = course.Sessions.Select(s => s.Attendances);
            var attendances = sessions.SelectMany(a => a).Where(a => a.StudentId == student.Id).Select(a => new CourseAttendanceDto
            {
                DateCreated = a.Session.CreatedAt.ToString("yyyy-MM-dd"),
                Status = true,
                TimeMarked = a.CreatedAt.ToString("HH:mm:ss")
            }).ToList();
            return new BaseResponse<ICollection<CourseAttendanceDto>>
            {
                Success = true,
                Message = "Sessions retrieved successfully",
                Data = attendances
            };

        }

        public async Task<BaseResponse<ICollection<StudentDto>>> GetCourseStudents(string courseName)
        {
            var course = await _courseRepository.Get(c => c.CourseCode == courseName);
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
                    Faculty = s.Student.Faculty,
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
    }
}