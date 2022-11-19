using SignalRSwaggerGen.Attributes;

namespace BattleshipDotNet.Hubs;

[SignalRHub]
public interface IRoomHubClient
{
    Task GetAllRooms(IEnumerable<string> roomNames);
}
