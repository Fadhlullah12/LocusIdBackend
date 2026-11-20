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
        public FacultyService(IDecodeTokenService decodeTokenService, IFacultyRepository facultyRepository)
        {
            _decodeTokenService = decodeTokenService;
            _facultyRepository = facultyRepository;
        }

        public async Task<BaseResponse<FacultyDto>> CreateFaculty(CreateFacultyRequestModel request, string token)
        {
            var existingFaculty = await _facultyRepository.Get(c => c.Name == request.Name);
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
                 Name = request.Name
             };
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

        public async Task<BaseResponse<ICollection<FacultyDto>>> GetFaculties()
        {
            var faculties = await _facultyRepository.GetFaculties(_ => true);
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