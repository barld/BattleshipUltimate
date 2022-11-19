using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace BattleshipDotNet.Hubs;

/// <summary>
/// The signalR hub to manage the play rooms
/// </summary>
[SignalRHub]
public class RoomHub : Hub<IRoomHubClient>
{
    private static IEnumerable<string> roomNames = new[] {"testRoom"};

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        await Clients.Caller.GetAllRooms(roomNames);
    }

    /// <summary>
    /// Create a new play room
    /// </summary>
    /// <param name="roomName">Name of the new room.</param>
    /// <returns></returns>
    public async Task CreateRoom(string roomName)
    {
        roomNames = roomNames.Append(roomName);
        await Clients.All.GetAllRooms(roomNames);
    }
}
