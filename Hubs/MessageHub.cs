using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LocusIDBackend.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var courseId = httpContext?.Request.Query["courseId"].ToString();
            if (!string.IsNullOrEmpty(courseId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"course-{courseId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var courseId = httpContext?.Request.Query["courseId"].ToString();
            if (!string.IsNullOrEmpty(courseId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"course-{courseId}");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
