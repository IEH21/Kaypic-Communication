using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Web3_kaypic.Data;
using Web3_kaypic.Hub;
using Web3_kaypic.Models;

namespace Web3_kaypic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hub;

        public VoteController(ApplicationDbContext context, IHubContext<NotificationHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        [HttpPost]
        public async Task<IActionResult> Voter([FromBody] Vote vote)
        {
            var dejaVote = await _context.Votes
                .AnyAsync(v => v.SondageId == vote.SondageId && v.UtilisateurId == vote.UtilisateurId);
            if (dejaVote) return BadRequest("Vous avez déjà voté.");

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            var resultats = await _context.Votes
                .Where(v => v.SondageId == vote.SondageId)
                .GroupBy(v => v.OptionId)
                .Select(g => new { OptionId = g.Key, Total = g.Count() })
                .ToListAsync();

            await _hub.Clients.All.SendAsync("UpdateVotes", vote.SondageId, resultats);
            return Ok("Vote enregistré.");
        }
    }
}
