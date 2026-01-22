using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web3_kaypic.Data;
using Microsoft.AspNetCore.Authorization;

namespace Web3_kaypic.Controllers
{
    [Authorize]
    public class CircleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CircleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Circle()
        {
            var messages = await _context.TMessages
                .Include(m => m.Comments) //commentaires
                .OrderByDescending(m => m.msg_created_at)
                .ToListAsync();

            return View(messages);
        }
    }
}