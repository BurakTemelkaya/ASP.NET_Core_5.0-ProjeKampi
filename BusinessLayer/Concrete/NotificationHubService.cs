using BusinessLayer.Abstract;
using CoreLayer.Extensions;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete;

public class NotificationHubService : INotificationHubService
{
    private readonly IHubContext<SignalRHub> _hubContext;

    public NotificationHubService(IHubContext<SignalRHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotification(int userId, string message)
    {
        var connectionIds = SignalRHub.GetConnectionId(userId);
        if (connectionIds != null && connectionIds.Count > 0)
        {
            List<Task> tasks = new();
            foreach (var connectionId in connectionIds)
            {
                tasks.Add(_hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", message));
            }
            await Task.WhenAll(tasks);
        }
    }
}
