namespace LocusIDBackend.Models.Entities
{
    public class MessageReadReceipts : BaseEntity
    {
        // IDs are strings because BaseEntity.Id is a string (GUID)
        public string MessageId { get; set; } = default!;
        public string StudentId { get; set; } = default!;
        public Message Message { get; set; } = default!;
        public Student Student { get; set; } = default!;
        public DateTime ReadAt { get; set; } = DateTime.UtcNow;
    }
}