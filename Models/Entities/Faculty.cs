namespace LocusIDBackend.Models.Entities
{
    public class Faculty : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string SchoolId { get; set; } = default!;
        public School School { get; set; } = default!;
        public List<Department> Departments { get; set; } = new List<Department>();
    }
}