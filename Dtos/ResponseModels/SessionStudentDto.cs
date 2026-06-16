namespace LocusIDBackend.Dtos.ResponseModels
{
    public class SessionStudentDto
    {
        public string SessionId { get; set; } = default!;
        public ICollection<StudentDto>? Students { get; set; }
    }
}