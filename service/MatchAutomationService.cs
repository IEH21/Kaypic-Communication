using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Web3_kaypic.Data;
using Web3_kaypic.Hub;

using Web3_kaypic.Models;

namespace Web3_kaypic.Services
{
    public class MatchAutomationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _email;
        private readonly IHubContext<NotificationHub> _hub;

        public MatchAutomationService(ApplicationDbContext context, IEmailService email, IHubContext<NotificationHub> hub)
        {
            _context = context;
            _email = email;
            _hub = hub;
        }

        // ✅ Ticket 12 — rappel 24h avant
        public async Task EnvoyerRappels24hAsync()
        {
            var demain = DateTime.Now.AddHours(24);
            var matchs = await _context.Matches
                .Where(m => !m.Rappel24hEnvoye && m.DateMatch.Date == demain.Date)
                .ToListAsync();

            foreach (var match in matchs)
            {
                await _email.SendEmailAsync(match.EmailCoach, "Rappel match demain",
                    $"Votre match {match.Equipe} est prévu demain à {match.DateMatch:hh:mm tt} à {match.Lieu}.");
                await _hub.Clients.All.SendAsync("ReceiveNotification", $"📅 Rappel : match {match.Equipe} demain !");
                match.Rappel24hEnvoye = true;
            }
            await _context.SaveChangesAsync();
        }

        // ✅ Ticket 13 — rappel jour J
        public async Task EnvoyerRappelsJourJAsync()
        {
            var aujourd = DateTime.Now.Date;
            var matchs = await _context.Matches
                .Where(m => !m.RappelJourJEnvoye && m.DateMatch.Date == aujourd)
                .ToListAsync();

            foreach (var match in matchs)
            {
                await _email.SendEmailAsync(match.EmailCoach, "Match aujourd’hui !",
                    $"Match de {match.Equipe} aujourd’hui à {match.DateMatch:hh:mm tt}, lieu : {match.Lieu}, équipement : {match.Equipement}.");
                await _hub.Clients.All.SendAsync("ReceiveNotification", $"⚽ Match de {match.Equipe} aujourd’hui !");
                match.RappelJourJEnvoye = true;
            }
            await _context.SaveChangesAsync();
        }
    }
}
