public class SessionDto
{
    public string Id { get; set;} = default!;
    public string CourseName { get; set; } = default!;
    public string LecturerName { get; set; } = default!;
    public bool IsActive { get; set; } 
    public string StartTime { get; set; } = default!;
    public string? DurationMinutes { get; set; } 
}