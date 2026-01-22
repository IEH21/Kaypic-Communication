using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web3_kaypic.Models;
using Web3_kaypic.Data;
using Web3_kaypic.Models;

namespace Web3_kaypic.Controllers
{
    public class InscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public InscriptionController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ---- PAGE D'INSCRIPTION ----
        [HttpGet]
        public IActionResult Inscription()
        {
            return View("~/Views/Home/Inscription.cshtml");
        }

        // ---- TRAITEMENT DU FORMULAIRE ----
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscription(Inscription model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Tous les champs doivent être remplis correctement.";
                return View("~/Views/Home/Inscription.cshtml", model);
            }

            // Vérifier si l'utilisateur existe déjà
            var existingUser = await _userManager.FindByEmailAsync(model.Courriel);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Ce courriel est déjà utilisé.";
                return View("~/Views/Home/Inscription.cshtml", model);
            }

            // Créer un nouvel utilisateur Identity
            var user = new ApplicationUser
            {
                UserName = model.Courriel,   // Identity utilise UserName pour la connexion
                Email = model.Courriel,
                PhoneNumber = model.NumeroTelephone,
                FirstName = model.Prenom,
                LastName = model.Nom
            };

            var result = await _userManager.CreateAsync(user, model.MotDePasse);

            if (result.Succeeded)
            {
                // Connexion automatique après inscription
                //await _signInManager.SignInAsync(user, isPersistent: false);

                // Sauvegarder aussi dans ta table personnalisée "Inscriptions" si tu l’as définie
                if (_context.Inscription != null)
                {
                    _context.Inscription.Add(model);
                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = "Inscription réussie, vous pouvez maintenant vous connecter.";
                return RedirectToAction("Connexion", "Connexion");
            }
            else
            {
                // Afficher les erreurs exactes d’Identity (mot de passe trop faible, etc.)
                ViewBag.ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return View("~/Views/Home/Inscription.cshtml", model);
            }
        }
    }
}
