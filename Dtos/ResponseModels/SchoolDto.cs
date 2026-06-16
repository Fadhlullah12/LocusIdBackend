namespace LocusIDBackend.Dtos.ResponseModels
{
    public class SchoolDto
    {
        public string Name { get; set; } = default!;
        public ICollection<StudentDto> Students { get; set; } = new HashSet<StudentDto>();
        public ICollection<FacultyDto> Faculties { get; set; } = new HashSet<FacultyDto>();
        public ICollection<CreateLecturerResponseDto> Lecturers { get; set; } = new HashSet<CreateLecturerResponseDto>();
        public ICollection<AcademicSessionDto> AcademicSessions { get; set; } = new HashSet<AcademicSessionDto>();

    }
}