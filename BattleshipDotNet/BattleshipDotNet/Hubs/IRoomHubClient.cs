using SignalRSwaggerGen.Attributes;

namespace BattleshipDotNet.Hubs;

[SignalRHub]
public interface IRoomHubClient
{
    void RoomCreated();
}
