using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class LecturerService : ILecturerService
    {
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDecodeTokenService _decodeTokenService;
        private readonly ISchoolRepository _schoolRepository;
        public LecturerService(ILecturerRepository lecturerRepository, IUserRepository userRepository, IDecodeTokenService decodeTokenService, ISchoolRepository schoolRepository)
        {
            _lecturerRepository = lecturerRepository;
            _userRepository = userRepository;
            _decodeTokenService = decodeTokenService;
            _schoolRepository = schoolRepository;
        }

        public async Task<BaseResponse<CreateLecturerResponseDto>> CreateLecturer(CreateLecturerRequestModel request)
        {
            var existinglecturer = await _lecturerRepository.Get(l => l.StaffId == request.LecturerId);
            var existingUser = await _userRepository.Get(s => s.Email == request.Email);
            var school = await _schoolRepository.Get(s => s.Name == request.SchoolName);
            var department = school.Faculties.SelectMany(f => f.Departments).FirstOrDefault(d => d.Name == request.Department);
             if (school == null)
            {
                return new BaseResponse<CreateLecturerResponseDto>
                {
                    Success = false,
                    Message = "School not found"
                };
            }
                if (department == null)
                {
                    return new BaseResponse<CreateLecturerResponseDto>
                    {
                        Success = false,
                        Message = "Department not found"
                    };
                }
            if (existinglecturer != null)
            {
                return new BaseResponse<CreateLecturerResponseDto>
                {
                    Success = false,
                    Message = "Lecturer with the same staff ID already exists"
                };
            }
             if (existingUser != null)
            {
                return new BaseResponse<CreateLecturerResponseDto>
                {
                    Success = false,
                    Message = "User with the same email already exists"
                };
            }
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Lecturer",
                Email = request.Email,
            };
            var lecturer = new Lecturer
            {
                StaffId = request.LecturerId,
                User = user,
                UserId = user.Id,
                School = school,
                SchoolId = school.Id,
                DepartmentId = department.Id,
                Department = department,
            };
            user.Lecturer = lecturer;
            await _lecturerRepository.Create(lecturer);
            await _userRepository.Create(user);
            await _lecturerRepository.Save();
            return new BaseResponse<CreateLecturerResponseDto>
            {
                Success = true,
                Message = "Lecturer created successfully",
                Data = new CreateLecturerResponseDto
                {
                    FirstName = lecturer.User.FirstName,
                    LastName = lecturer.User.LastName,
                    Role = lecturer.User.Role,
                    PhotoUrl = lecturer.User.PhotoUrl
                }
            };
        }

        public async Task<BaseResponse<ICollection<CourseDto>>> GetLecturerCourses(string token)
        {
            string userId = _decodeTokenService.GetIdFromRawToken(token);
            if (userId == null)
            {
                return new BaseResponse<ICollection<CourseDto>>
                {
                    Success = false,
                    Message = "Invalid token",
                };
            }
            var lecturer = await _lecturerRepository.Get(l => l.UserId == userId);

            if (lecturer == null)
            {
                return new BaseResponse<ICollection<CourseDto>>
                {
                    Success = false,
                    Message = "Lecturer not found",
                };
            }

            var courses = lecturer.Courses?.Select(c => new CourseDto
            {
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                CreatedAt = c.CreatedAt,
                
            }).ToList() ?? new List<CourseDto>();

            return new BaseResponse<ICollection<CourseDto>>
            {
                Success = true,
                Message = "Courses retrieved successfully",
                Data = courses
            };
        }
    }
}