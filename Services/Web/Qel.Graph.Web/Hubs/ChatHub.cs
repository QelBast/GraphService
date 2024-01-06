using Microsoft.AspNetCore.SignalR;

namespace Qel.Graph.Web.Hubs;

public class ChatHub : Hub
{
    public void Send(string name, string message)
    {
        // Call the addNewMessageToPage method to update clients.
       // Clients.All.func(name, message);
    }
}
