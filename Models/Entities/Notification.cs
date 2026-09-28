namespace LocusIDBackend.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string? SenderName { get; set; } // lecturer name
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}