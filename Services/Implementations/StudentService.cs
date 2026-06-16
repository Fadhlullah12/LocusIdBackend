using System.Collections;
using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDecodeTokenService _dectoken;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IDepartmentRepository _departmentRepository;
        public StudentService(IStudentRepository studentRepository, IUserRepository userRepository, IDecodeTokenService decodeTokenService,
         ICourseRepository courseRepository, IStudentCourseRepository studentCourseRepository, ISchoolRepository schoolRepository, IDepartmentRepository departmentRepository)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _dectoken = decodeTokenService;
            _courseRepository = courseRepository;
            _studentCourseRepository = studentCourseRepository;
            _schoolRepository = schoolRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<BaseResponse<StudentDto>> CreateStudent(CreateStudentRequestModel request)
        {
            var existingstudent = await _studentRepository.Get(s => s.MatricNumber == request.MatricNumber);
            var existingUser = await _userRepository.Get(s => s.Email == request.Email);
            var school = await _schoolRepository.Get(s => s.Name == request.SchoolName);
            var department = await _departmentRepository.Get(d => d.Name == request.Department);
             if(department == null)
            {
                return new BaseResponse<StudentDto>
                {
                    Success = false,
                    Message = "Department not found"
                };
            }
             int [] ints = new int[6];
            ints = [1,2,3];
            int [] ints2 = [1,2,3];
            ints.Concat(ints2);
            if(department != null)
            {
                return new BaseResponse<StudentDto>
                {
                    Success = false,
                    Message = "User with similar Email already exists"
                };
            }
            if (existingstudent != null)
            {
                return new BaseResponse<StudentDto>
                {
                    Success = false,
                    Message = "Student with the same matric number already exists"
                };
            }
            if (school == null)
            {
                return new BaseResponse<StudentDto>
                {
                    Success = false,
                    Message = "School not found"
                };
            }
            var student = new Student
            {
                Id = Guid.NewGuid().ToString(),
                MatricNumber = request.MatricNumber,
                Department = department,
                DepartmentId = department.Id,
                Faculty = request.Faculty,
                School = school,
                SchoolId = school.Id,
            };
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                Student = student,
                Email = request.Email,
            };
            school.Students.Add(student);
            student.User = user;
            student.UserId = user.Id;
            await _studentRepository.Create(student);
            await _userRepository.Create(user);
            await _studentRepository.Save();
            return new BaseResponse<StudentDto>
            {
                Success = true,
                Message = "Student created successfully",
                Data = new StudentDto
                {
                    Id = student.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    MatricNumber = student.MatricNumber,
                    Department = department.Name,
                    Faculty = student.Faculty
                }
            };

        }

        public async Task<BaseResponse<string>> DropCourses(ICollection<string> courseIds, string token)
        {
            var userId = _dectoken.GetIdFromRawToken(token);
            if (userId == null)
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Invalid token",
                };
            }

            var student = await _studentRepository.Get(s => s.UserId == userId);
            if (student == null)
            {
                return new BaseResponse<string> { Success = false, Message = "Student not found" };
            }

            var studentCoursesToRemove = student.StudentCourses.Where(sc => courseIds.Contains(sc.CourseId)).ToList();

            if (!studentCoursesToRemove.Any())
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "You are not enrolled in any of the selected courses."
                };
            }

            _studentCourseRepository.DeleteRange(studentCoursesToRemove);
            await _studentCourseRepository.Save();

            return new BaseResponse<string>
            {
                Success = true,
                Message = "Courses dropped successfully"
            };
        }

        public async Task<BaseResponse<ICollection<CourseDto>>> EnrollCourse(EnrollCourseRequestModel model)
        {
            var userId = _dectoken.GetIdFromRawToken(model.Token);
            if (userId == null)
            {
                return new BaseResponse<ICollection<CourseDto>>
                {
                    Success = false,
                    Message = "Invalid token",
                };
            }

            var student = await _studentRepository.Get(s => s.UserId == userId);
            if (student == null)
            {
                return new BaseResponse<ICollection<CourseDto>> { Success = false, Message = "Student not found" };
            }

            var existingCourseIds = student.StudentCourses
            .Select(sc => sc.CourseId.ToString())
            .ToList();

            var newCourseIds = model.CourseIds.Except(existingCourseIds).ToList();

            if (!newCourseIds.Any())
            {
                return new BaseResponse<ICollection<CourseDto>>
                {
                    Success = false,
                    Message = "You are already enrolled in all selected courses."
                };
            }

            var coursesToAdd = await _courseRepository.GetCoursesByIds(newCourseIds);

            foreach (var course in coursesToAdd)
            {
                var studentCourse = new StudentCourse
                {
                    StudentId = student.Id,
                    CourseId = course.Id,
                    Course = course,
                    Student = student
                };
                await _studentCourseRepository.Create(studentCourse);
                course.CourseStudents.Add(studentCourse);
                student.StudentCourses.Add(studentCourse);
            }

           
            await _studentRepository.Update(student);
            await _studentRepository.Save();
            var enrolledCourses = student.StudentCourses.Select(sc => new CourseDto
            {
                CourseCode = sc.Course.CourseCode,
                CourseName = sc.Course.CourseName
            }).ToList();
            return new BaseResponse<ICollection<CourseDto>>
            {
                Success = true,
                Message = "Courses enrolled successfully",
                Data = enrolledCourses
            };

        }

        public async Task<BaseResponse<ICollection<CourseDto>>> GetStudentCourses(string token)
        {
             var userId = _dectoken.GetIdFromRawToken(token);
            if (userId == null)
            {
                return new BaseResponse<ICollection<CourseDto>>
                {
                    Success = false,
                    Message = "Invalid token",
                };
            }

            var student = await _studentRepository.Get(s => s.UserId == userId);
            if (student == null)
            {
                return new BaseResponse<ICollection<CourseDto>> { Success = false, Message = "Student not found" };
            }
            var courses = student.StudentCourses.Select(sc => new CourseDto
            {
                CourseCode = sc.Course.CourseCode,
                CourseName = sc.Course.CourseName
            }).ToList();
            return new BaseResponse<ICollection<CourseDto>>
            {
                Success = true,
                Message = "Courses retrieved successfully",
                Data = courses
            };

        }
    }
}