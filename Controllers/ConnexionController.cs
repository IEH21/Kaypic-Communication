using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web3_kaypic.Models;
using Web3_kaypic.Models;

namespace Web3_kaypic.Controllers
{
    public class ConnexionController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConnexionController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Connexion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Connexion(connexion model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "⚠️ Veuillez entrer toutes les informations.";
                return View(model);
            }

            // Vérifier si l'utilisateur existe
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Error"] = "Email ou mot de passe incorrect.";
                return View(model);
            }

            // Connexion avec Identity
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,   // ⚠️ Identity utilise UserName
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "✅ Connexion réussie.";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Connexion impossible.";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "✅ Vous êtes déconnecté.";
            return RedirectToAction("Index", "Home");
        }
    }
}
