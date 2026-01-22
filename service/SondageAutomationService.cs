using Microsoft.EntityFrameworkCore;
using Web3_kaypic.Data;
using Web3_kaypic.Services;
using Microsoft.AspNetCore.SignalR;
using Web3_kaypic.Hub;

namespace Web3_kaypic.Services
{
    public class SondageAutomationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _email;
        private readonly IHubContext<NotificationHub> _hub;

        public SondageAutomationService(ApplicationDbContext context, IEmailService email, IHubContext<NotificationHub> hub)
        {
            _context = context;
            _email = email;
            _hub = hub;
        }

        // ✅ Ticket 18 — rappels sondages
        public async Task EnvoyerRappelsSondagesAsync()
        {
            var sondages = await _context.Sondages.ToListAsync();
            foreach (var sondage in sondages)
            {
                var heuresRestantes = (sondage.Expiration - DateTime.Now).TotalHours;
                if (heuresRestantes < 24 && heuresRestantes > 23) // environ 24h avant
                {
                    await _hub.Clients.All.SendAsync("ReceiveNotification",
                        $"🗳️ Rappel : le sondage '{sondage.Question}' se termine demain !");
                }
            }
        }
    }
}
