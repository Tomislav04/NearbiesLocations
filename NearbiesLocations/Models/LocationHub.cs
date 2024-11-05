using Microsoft.AspNetCore.SignalR;

namespace NearbiesLocations.Models
{
    public class LocationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
