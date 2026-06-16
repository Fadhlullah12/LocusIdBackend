namespace LocusIDBackend.Dtos.RequestModels
{
    public class CreateDepartmentRequestModel
    {
        public string Name { get; set; } = default!;
        public string FacultyId { get; set; } = default!;
    }
}