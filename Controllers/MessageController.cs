using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Web3_kaypic.Data;
using Web3_kaypic.Hub;
using web3_kaypic.Models;

namespace Web3_kaypic.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageController(ApplicationDbContext context, IWebHostEnvironment env, IHubContext<MessageHub> hubContext)
        {
            _context = context;
            _env = env;
            _hubContext = hubContext;
        }

        // --- Vue principale : liste des chats avec leurs messages, personas et médias
        public async Task<IActionResult> Messages(string? searchTerm)
        {
            var query = _context.TMessagingChat
                .Include(c => c.ChatPersonas)
                    .ThenInclude(cp => cp.Persona)
                .Include(c => c.Messages)
                .Include(c => c.Medias)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.ChatPersonas
                    .Any(p =>
                        p.Persona.mp_fname.Contains(searchTerm) ||
                        p.Persona.mp_lname.Contains(searchTerm)
                    ));
            }

            var chats = await query
                .OrderByDescending(c => c.Messages
                    .OrderByDescending(m => m.created_at)
                    .Select(m => m.created_at)
                    .FirstOrDefault())
                .ToListAsync();

            return View(chats);
        }

        // --- Upload de média ---
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int mc_id, int ts_id, int created_by_mp_id)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Aucun fichier reçu");

            try
            {
                // 1. Sauvegarde physique
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var uniqueName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploads, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 2. Déterminer la catégorie
                var extension = Path.GetExtension(file.FileName).ToLower();
                var category = extension switch
                {
                    ".jpg" or ".jpeg" or ".png" or ".gif" => "I",
                    ".mp4" or ".webm" => "V",
                    _ => "D"
                };

                // 3. Enregistrement en DB
                var media = new TMessagingMedia
                {
                    mc_id = mc_id,
                    ts_id = ts_id,
                    created_by_mp_id = created_by_mp_id,
                    mcm_media_category = category,
                    mcm_url = "/uploads/" + uniqueName,
                    mcm_filename = file.FileName,
                    mcm_size = file.Length,
                    created_at = DateTime.UtcNow
                };

                _context.TMessagingMedia.Add(media);
                await _context.SaveChangesAsync();

                // 4. Notification temps réel via SignalR
                await _hubContext.Clients.Group($"chat:{mc_id}")
                    .SendAsync("ReceiveMedia", new
                    {
                        media.mcm_id,
                        media.mc_id,
                        media.ts_id,
                        media.mcm_url,
                        media.mcm_media_category,
                        media.created_by_mp_id,
                        media.created_at,
                        media.mcm_filename,
                        media.mcm_size
                    });

                // 5. Retour HTTP
                return Ok(media);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de l'upload : {ex.Message}");
            }
        }

        // --- Vue Annonces ---
        public IActionResult Annonces() => View();

        // --- Vue Groupe ---
        public IActionResult Groupe() => View();

        // --- Vue Messages personnels (self notes) ---
        public IActionResult MessagesSelf() => View();

        // --- 📄 Détails d’un message ---
        public async Task<IActionResult> Details(long id)
        {
            var message = await _context.TMessagingChatPersonaMessage
                .Include(m => m.ChatPersona)
                .FirstOrDefaultAsync(m => m.mcpm_id == id);

            if (message == null)
                return NotFound();

            return View(message);
        }
    }
}
