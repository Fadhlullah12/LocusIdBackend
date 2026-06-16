using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Services.Implementations
{
    public class FacultyService : IFacultyService
    {
        private readonly IDecodeTokenService _decodeTokenService;   
        private readonly IFacultyRepository _facultyRepository;
        private readonly IDirectorRepository _directorRepository;
        public FacultyService(IDecodeTokenService decodeTokenService, IFacultyRepository facultyRepository,IDirectorRepository directorRepository)
        {
            _decodeTokenService = decodeTokenService;
            _facultyRepository = facultyRepository;
            _directorRepository = directorRepository;
        }

        public async Task<BaseResponse<FacultyDto>> CreateFaculty(CreateFacultyRequestModel request, string token)
        {
            var userID ="7feca7a2-4b12-4314-b03c-c13fb8e3c58c";
            var director = await  _directorRepository.Get(u => u.UserId == userID);
            var school = director.School;
            var existingFaculty = await _facultyRepository.Get(c => c.Name == request.Name && c.SchoolId == director.School.Id);
            if(existingFaculty != null)
            {
                return new BaseResponse<FacultyDto>
                {
                    Success = false,
                    Message = "Faculty already exists"
                };
            }
             var faculty = new Faculty
             {
                 Name = request.Name,
                 School = director.School,
                 SchoolId = director.School.Id,
             };
             school.Faculties.Add(faculty);
             await _facultyRepository.Create(faculty);
             await _facultyRepository.Save();
             return new BaseResponse<FacultyDto>
             {
                 Data = new FacultyDto
                 {
                    Name = faculty.Name,                    
                 },
                 Message = $"Faculty {faculty.Name} Creadted Sucessfully",
                 Success = true,
             };
        }

        public async Task<BaseResponse<ICollection<FacultyDto>>> GetFaculties(string schoolName)
        {
            var faculties = await _facultyRepository.GetFaculties(c => c.School.Name == schoolName);
            var facultyDtos = faculties.Select(a => new FacultyDto
            {
              Name = a.Name,
            }).ToList();
            return new BaseResponse<ICollection<FacultyDto>>
            {
              Message = $"Faculties Retrived sucessfully",
              Success = true,
              Data = facultyDtos
            };
        }
    }
}