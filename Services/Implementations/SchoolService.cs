using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IUserRepository _userRepository;
        public SchoolService(ISchoolRepository schoolRepository, IUserRepository userRepository, IDirectorRepository directorRepository)
        {
            _schoolRepository = schoolRepository;
            _userRepository = userRepository;
            _directorRepository = directorRepository;
        }

        public async Task<BaseResponse<SchoolDto>> CreateSchool(CreateSchoolRequestModel request)
        {
           var existingUser = await _userRepository.Get(a => a.Email == request.Email);
           var existingSchool = await _schoolRepository.Get(a => a.Name == request.SchoolName);
            if (existingSchool != null)
            {
                return new BaseResponse<SchoolDto>()
                {
                    Success = false,
                    Message = $"School with name {request.SchoolName} is already exists.",
                };
            }
              if (existingUser != null)
            {
                return new BaseResponse<SchoolDto>()
                {
                    Success = false,
                    Message = $"User with email {request.Email} is already exists.",
                };
            }
             var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Director",
            };
            var director = new Director
            {
                UserId = user.Id,
                User = user,
                StaffId = request.StaffId,
            };
            var school = new School
            {
              Name = request.SchoolName,
              Director = director,
              DirectorId = director.Id,
            };
            director.School = school;
            user.Director = director;
            await _userRepository.Create(user);
            await _directorRepository.Create(director);
            await _schoolRepository.Create(school);
            await _schoolRepository.Save();
            return new BaseResponse<SchoolDto>()
            {
                Success = true,
                Message = "School created successfully",
                Data = new SchoolDto
                {
                    Name = request.SchoolName,
                }
            };

        }

        public async Task<BaseResponse<ICollection<SchoolDto>>> GetSchools()
        {
            var schools = await _schoolRepository.GetAll();
            var schoolDto = schools.Select(a => new SchoolDto
            {
               Name = a.Name,
            }).ToList();
            return new BaseResponse<ICollection<SchoolDto>>
           {
               Success = true,
               Data = schoolDto,
               Message = "School retreived"
           };
        }
    }
}