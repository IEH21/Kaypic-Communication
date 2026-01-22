using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web3_kaypic.Models.Finance.DTOs;
using Web3_kaypic.Services.Finance;
using Microsoft.AspNetCore.Identity;
using web3_kaypic.Models;

namespace Web3_kaypic.Controllers
{
    // L'ATTRIBUE [Authorize] protège TOUS les endpoints de ce contrôleur
    // L'utilisateur DOIT être connecté pour accéder à ces actions
    [Authorize]
    public class StripeController : Controller
    {
        // Service Stripe pour créer les sessions de paiement
        private readonly StripeService _stripeService;
        
        // Gestionnaire Identity pour récupérer les infos utilisateur connecté
        private readonly UserManager<ApplicationUser> _userManager;

        public StripeController(StripeService stripeService, UserManager<ApplicationUser> userManager)
        {
            _stripeService = stripeService;
            _userManager = userManager;
        }

        // ============================================================
        //  LANCER LE PAIEMENT STRIPE (POST)
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Checkout(decimal amount)
        {
            // Récupère l'utilisateur actuellement connecté via Identity
            var user = await _userManager.GetUserAsync(User);

            // Prépare les données pour le service Stripe
            var dto = new StripeCheckoutDto
            {
                Amount = amount, // Montant saisi depuis le formulaire
                // URLs dynamiques construites avec le protocole HTTP/HTTPS actuel
                SuccessUrl = Url.Action("Success", "Stripe", null, Request.Scheme),
                CancelUrl = Url.Action("Cancel", "Stripe", null, Request.Scheme)
            };

            // Crée la session Stripe et récupère l'URL de paiement
            var url = await _stripeService.CreateCheckoutSessionAsync(user.Id, dto);

            // REDIRIGE directement vers la page de paiement Stripe
            return Redirect(url);
        }

        // ============================================================
        //  PAGE DE SUCCÈS (après paiement réussi)
        // ============================================================
        public IActionResult Success() => View();

        // ============================================================
        //  PAGE D'ANNULATION (paiement abandonné)
        // ============================================================
        public IActionResult Cancel() => View();
    }
}
