namespace LocusIDBackend.Dtos.ResponseModels
{
    public class DepartmentDto
    {
        
        public string Name { get; set; } = default!;
        public string FacultyName { get; set; } = default!;
        public ICollection<CreateLecturerResponseDto>? Lecturers { get; set; } = new HashSet<CreateLecturerResponseDto>();
        // public ICollection<DepartmentCourse>? DepartmentCourses { get; set;} = new HashSet<DepartmentCourse>();
    }
}