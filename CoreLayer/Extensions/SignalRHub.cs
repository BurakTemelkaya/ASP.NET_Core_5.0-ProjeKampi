using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayer.Extensions;

public class SignalRHub : Hub
{
    private static readonly ConcurrentDictionary<int, string> _userConnections = new ConcurrentDictionary<int, string>();
    private readonly UserHelper _userHelper;

    public SignalRHub(UserHelper userHelper)
    {
        _userHelper = userHelper;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = _userHelper.GetUserId();

        _userConnections[userId] = Context.ConnectionId;

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = _userHelper.GetUserId();

        _userConnections.TryRemove(userId, out _);

        await base.OnDisconnectedAsync(exception);
    }

    public static List<string> GetConnectionId(int userId)
    {
       return _userConnections.Where(x => x.Key == userId).Select(x=> x.Value).ToList();
    }
}
