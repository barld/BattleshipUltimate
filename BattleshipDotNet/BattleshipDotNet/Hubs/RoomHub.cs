using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace BattleshipDotNet.Hubs;

[SignalRHub]
public interface IRoomClient
{
    Task GetAllRooms(IEnumerable<string> roomNames);
}

[SignalRHub]
public class RoomHub : Hub<IRoomClient>
{
    private IEnumerable<string> roomNames = Enumerable.Empty<string>();

    public async Task CreateRoom(string roomName)
    {
        roomNames = roomNames.Append(roomName);
        await Clients.All.GetAllRooms(roomNames);
    }
}
