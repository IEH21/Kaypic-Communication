using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web3_kaypic.Models;
using Web3_kaypic.Models.Finance.DTOs;
using Web3_kaypic.Services.Finance;

namespace Web3_kaypic.Controllers.API
{
    // TOUS les endpoints nécessitent une authentification JWT
    [Authorize]
    // Route de base : /api/pay
    [Route("api/pay")]
    [ApiController]
    public class PaymentsApiController : ControllerBase
    {
        // Service Stripe pour créer les sessions de paiement
        private readonly StripeService _stripe;
        
        // Gestionnaire Identity pour identifier l'utilisateur connecté
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentsApiController(StripeService stripe, UserManager<ApplicationUser> userManager)
        {
            _stripe = stripe;
            _userManager = userManager;
        }

        // ============================================================
        //  MÉTHODE PRIVÉE : RÉCUPÉRER L'ID DE L'UTILISATEUR CONNECTÉ
        // ============================================================
        private async Task<string> GetUserId()
        {
            // Récupère l'utilisateur depuis le token JWT (Claims)
            var user = await _userManager.GetUserAsync(User);
            return user.Id;
        }

        // ============================================================
        //  CRÉER UNE SESSION STRIPE (AJAX)
        //  POST /api/pay/create-session
        // ============================================================
        [HttpPost("create-session")]
        public async Task<IActionResult> CreateSession([FromBody] StripeCheckoutDto dto)
        {
            // Récupère l'ID de l'utilisateur connecté (via JWT)
            var userId = await GetUserId();

            // Crée la session Stripe via le service (enregistre aussi en BD)
            var url = await _stripe.CreateCheckoutSessionAsync(userId, dto);

            // Retourne l'URL Stripe en JSON pour le frontend
            return Ok(new { url });
        }
    }
}