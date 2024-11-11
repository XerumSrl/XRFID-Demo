using Microsoft.AspNetCore.SignalR;

namespace XRFID.Server.Demo.V2.Hubs;

public class UiMessageHub : Hub
{
    public async Task AddToGroup(string groupName)
       => await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

    public async Task RemoveFromGroup(string groupName)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    public async Task SendMessage()
    {

        await Clients.All.SendAsync("RefreshTag");
        await Clients.All.SendAsync("RefreshState");
        await Clients.All.SendAsync("ReceiveMessage");
    }
}
