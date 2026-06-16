public class StudentDto
{
    public string Id { get; set;} = default!;
    public string FullName { get; set; } = default!;
    public string MatricNumber { get; set; } = default!;
    public string Department { get; set; } = default!;
    public string Faculty { get; set; } = default!;
    public double AttendancePercentage { get; set; }
}