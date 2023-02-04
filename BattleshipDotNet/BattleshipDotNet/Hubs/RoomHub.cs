using BattleshipDotNet.Logic;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using System.Collections.Immutable;

namespace BattleshipDotNet.Hubs;

/// <summary>
/// The signalR hub to manage the play rooms
/// </summary>
[SignalRHub]
public class RoomHub : Hub<IRoomHubClient>
{
    private static SortedDictionary<int, Game> rooms = new SortedDictionary<int, Game>();

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task EnterRoom()
    {
        int roomId = !rooms.Any() ? 0 : rooms.Last().Value.Board2 is not null ? rooms.Last().Key + 1 : rooms.Last().Key;

        if (!rooms.ContainsKey(roomId))
        {
            rooms.Add(roomId, new Game
            {
                Board1 = new PlayerBoard
                {
                    ClientId = Context.ConnectionId,
                    Hits = ImmutableList<Hit>.Empty,
                    PlayerName = string.Empty,
                    Ships = ImmutableList<Ship>.Empty
                },
                Board2 = null
            });

            await this.Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

            this.Clients.Group(roomId.ToString()).RoomCreated();
        } 
        else
        {
            rooms[roomId] = rooms[roomId] with
            {
                Board2 = new PlayerBoard
                {
                    ClientId = Context.ConnectionId,
                    Hits = ImmutableList<Hit>.Empty,
                    PlayerName = string.Empty,
                    Ships = ImmutableList<Ship>.Empty
                }
            };
        }
    }
}
