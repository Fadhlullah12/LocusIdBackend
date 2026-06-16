namespace LocusIDBackend.Dtos.ResponseModels
{
    public class FacultyDto
    {
        public string Name { get; set; } = default!;
        public List<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    }
}