// using System;
// using LocusIDBackend.Services.SignalRHub;
// using Microsoft.AspNetCore.SignalR;

// namespace LocusIDBackend.Services.Implementations
// {
//     public class NotificationService
//     {
//         private readonly IHubContext<NotificationHub> _hub;
//         public NotificationService()
//         {
//         }

//         public async Task NotifyStudentAsync(string userId, string message, string senderName)
//         {
//             var notification = new Notification { UserId = userId, Message = message, SenderName = senderName, CreatedAt = DateTime.UtcNow };
//             _db.Notifications.Add(notification);
//             await _db.SaveChangesAsync();

//             await _hub.Clients.Group($"user-{userId}").SendAsync("ReceiveNotification", notification);
//         }
//     }
// }

