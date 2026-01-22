using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Web3_kaypic.Hub  // correspond à ton dossier
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public async Task EnvoyerNotification(string message)
        {
            await Clients.All.SendAsync("RecevoirNotification", message);
        }
    }
}
