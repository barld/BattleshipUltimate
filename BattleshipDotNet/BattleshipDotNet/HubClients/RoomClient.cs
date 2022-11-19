using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BattleshipDotNet.HubClients;

public class RoomClient : SignalRClientBase
{
    public RoomClient(NavigationManager navigationManager) : base(navigationManager, "/Rooms")
    {
    }

    public async Task CreateRoom(string roomName)
    {
        await HubConnection.SendAsync(nameof(CreateRoom), roomName);
    }

    public void OnGetAllRooms(Action<IEnumerable<string>> action) 
    {
        HubConnection.On("GetAllRooms", action);
    }
}
