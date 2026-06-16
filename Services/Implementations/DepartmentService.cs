using LocusIDBackend.Dtos;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
using LocusIDBackend.Services.Interfaces;
using LocusIDBackend.Repositories.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDecodeTokenService _decodeTokenService;
        private readonly IDirectorRepository _directorRepository;
        private readonly IFacultyRepository _facultyRepository;
        public DepartmentService(IDepartmentRepository departmentRepository, IDecodeTokenService decodeTokenService,
                                  IDirectorRepository directorRepository, IFacultyRepository facultyRepository)
        {
            _departmentRepository = departmentRepository;
            _decodeTokenService = decodeTokenService;
            _directorRepository = directorRepository;
            _facultyRepository = facultyRepository;
        }

        public async Task<BaseResponse<DepartmentDto>> CreateDepartment(CreateDepartmentRequestModel request, string token)
        {
            var userId = "7feca7a2-4b12-4314-b03c-c13fb8e3c58c";
            var director = await _directorRepository.Get(a => a.UserId == userId);
            var faculty = await _facultyRepository.Get(a => a.Id == request.FacultyId);
            var existingDepartment = await _departmentRepository.Get(a => a.Name == request.Name && a.Faculty.SchoolId == director.School.Id);
            if (director == null)
            {
                return new BaseResponse<DepartmentDto>()
                {
                    Success = false,
                    Message = "Director not found",
                };
            }
            if (existingDepartment != null)
            {
                return new BaseResponse<DepartmentDto>()
                {
                    Success = false,
                    Message = $"Department with name {request.Name} is already exists.",
                };
            }
            if (faculty == null)
            {
                return new BaseResponse<DepartmentDto>()
                {
                    Success = false,
                    Message = "Faculty not found",
                };
            }
            var department = new Department
            {
                Name = request.Name,
                FacultyId = faculty.Id,
                Faculty = faculty,                
            };
            faculty.Departments.Add(department);
            await _departmentRepository.Create(department);
            await _departmentRepository.Save();
            return new BaseResponse<DepartmentDto>()
            {
                Success = true,
                Message = "Department created successfully",
                Data = new DepartmentDto
                {
                    Name = request.Name,
                }
            };
        }

        public async Task<BaseResponse<ICollection<FacultyDto>>> GetDepartmentsByFaculty(string facultyName)
        {
            var faculty = await _facultyRepository.Get(a => a.Name == facultyName);
            if (faculty == null)
            {
                return new BaseResponse<ICollection<FacultyDto>>()
                {
                    Success = false,
                    Message = "Faculty not found",
                };
            }

            var departments = faculty.Departments;
            var departmentDtos = departments.Select(d => new FacultyDto
            {
                Name = d.Name,
            }).ToList();

            return new BaseResponse<ICollection<FacultyDto>>()
            {
                Success = true,
                Message = "Departments retrieved successfully",
                Data = departmentDtos
            };
        }
    }
}