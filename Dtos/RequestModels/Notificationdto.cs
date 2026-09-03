namespace LocusIDBackend.Dtos.RequestModels
{
    public class NotificationDto
    {
         public string Id { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string CourseId { get; set; } = default!;
        public string CourseName { get; set; } = default!;
        public string LecturerName { get; set; } = default!;
    }
}