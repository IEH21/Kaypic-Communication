using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web3_kaypic.Models;

namespace Web3_kaypic.Controllers
{
    [Authorize]
    public class ProfilController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public ProfilController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        // Afficher le profil
        public async Task<IActionResult> Profil()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Connexion", "Connexion");
            }

            return View(user);
        }

        // Afficher le formulaire d'édition
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Connexion", "Connexion");
            }

            var model = new TEditProfile
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                CurrentProfileImageUrl = user.ProfileImageUrl
            };

            return View(model);
        }

        // Sauvegarder les modifications
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TEditProfile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Connexion", "Connexion");
            }

            // Mettre à jour les informations
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            // Gérer l'upload de la photo de profil
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(model.ProfileImage.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ProfileImage", "Format d'image invalide. Utilisez jpg, png ou gif.");
                    return View(model);
                }

                if (model.ProfileImage.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ProfileImage", "La taille de l'image ne doit pas dépasser 5MB.");
                    return View(model);
                }

                // Supprimer l'ancienne photo si elle existe
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, user.ProfileImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Sauvegarder la nouvelle photo
                var fileName = $"{user.Id}_{Guid.NewGuid()}{extension}";
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "profiles");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                user.ProfileImageUrl = $"/uploads/profiles/{fileName}";
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "✅ Profil mis à jour avec succès !";
                return RedirectToAction("Profil");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // Supprimer la photo de profil
        [HttpPost]
        public async Task<IActionResult> DeleteProfileImage()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Utilisateur non trouvé" });
            }

            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                var imagePath = Path.Combine(_env.WebRootPath, user.ProfileImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                user.ProfileImageUrl = null;
                await _userManager.UpdateAsync(user);
            }

            return Json(new { success = true });
        }
    }
}