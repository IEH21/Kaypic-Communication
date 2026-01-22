using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web3_kaypic.Data;
using Web3_kaypic.Models;

namespace Web3_kaypic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SondageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SondageController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("creer")]
        public async Task<IActionResult> Creer([FromBody] Sondage sondage)
        {
            _context.Sondages.Add(sondage);
            await _context.SaveChangesAsync();
            return Ok(sondage);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var sondage = await _context.Sondages.Include(s => s.Options).FirstOrDefaultAsync(s => s.Id == id);
            return sondage == null ? NotFound() : Ok(sondage);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sondages = await _context.Sondages.Include(s => s.Options).ToListAsync();
            return Ok(sondages);
        }
    }
}
