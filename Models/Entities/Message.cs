namespace LocusIDBackend.Models.Entities
{
    public class Message : BaseEntity
    {
        public string LecturerId { get; set; } = default!;
        public Lecturer Lecturer { get; set; } = default!;
        public string CourseId { get; set; } = default!;
        public Course Course { get; set; } = default!;
        public string Content { get; set; } = default!;
        public ICollection<MessageReadReceipts> ReadReceipts { get; set; } = new HashSet<MessageReadReceipts>();
    }
}

// namespace YourApp.Models.Entities
// {
//     public class Message
//     {
//         public int Id { get; set; }
//         public string SenderId { get; set; }   // Lecturer's UserId
//         public int CourseId { get; set; }
//         public string Content { get; set; }
//         public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//         // Navigation properties
//         public ApplicationUser Sender { get; set; }
        
//         public ICollection<MessageReadReceipt> ReadReceipts { get; set; }
//     }
// }