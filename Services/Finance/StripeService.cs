using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using Web3_kaypic.Data;
using Web3_kaypic.Models.Finance;
using Web3_kaypic.Models.Finance.DTOs;

namespace Web3_kaypic.Services.Finance
{
    public class StripeService
    {
        // Contexte de base de données pour enregistrer les transactions et paiements
        private readonly ApplicationDbContext _db;

        // Configuration pour accéder aux clés Stripe (dans appsettings.json)
        private readonly IConfiguration _config;

        public StripeService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // ================================
        //   CRÉER UNE SESSION CHECKOUT
        // ================================
        public async Task<string> CreateCheckoutSessionAsync(string userId, StripeCheckoutDto dto)
        {
            // Configuration des options de la session Stripe
            var options = new SessionCreateOptions
            {
                Mode = "payment", // Mode de paiement unique
                SuccessUrl = dto.SuccessUrl, // URL de succès
                CancelUrl = dto.CancelUrl,   // URL d’annulation
                PaymentMethodTypes = new List<string> { "card" }, // Méthodes acceptées

                // Détails des articles à payer (lignes de paiement)
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1, // une seule unité vendue
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = dto.Amount * 100, // Montant converti en cents
                            Currency = "cad", // devise canadienne
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Paiement Kaypic (Inscription/Frais)" // Nom affiché dans Stripe
                            }
                        }
                    }
                }
            };

            // Création réelle de la session Stripe via l’API
            var service = new SessionService();
            var session = await service.CreateAsync(options);

            // Enregistrement du paiement dans la base de données locale
            var payment = new StripePayment
            {
                UserId = userId,
                SessionId = session.Id,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow,
                Status = "pending" // statut initial avant validation
            };

            _db.StripePayments.Add(payment);
            await _db.SaveChangesAsync();

            // Retourne l’URL Stripe pour rediriger l’utilisateur
            return session.Url;
        }

        // ================================
        //      WEBHOOK STRIPE
        // ================================
        public async Task HandleWebhookAsync(HttpRequest request)
        {
            // Récupère le corps JSON envoyé par Stripe
            var json = await new StreamReader(request.Body).ReadToEndAsync();

            try
            {
                // Lecture de la signature sécurisée envoyée par Stripe
                var signature = request.Headers["Stripe-Signature"];
                var webhookSecret = _config["Stripe:WebhookSecret"];

                // Validation et construction de l’objet Stripe Event
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signature,
                    webhookSecret
                );

                // Vérifie si la session de paiement est complétée
                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;

                    // Recherche du paiement correspondant dans la BD
                    var payment = await _db.StripePayments
                        .FirstOrDefaultAsync(p => p.SessionId == session.Id);

                    // Si le paiement est trouvé et non encore validé
                    if (payment != null && payment.Status != "succeeded")
                    {
                        payment.Status = "succeeded";
                        payment.CompletedAt = DateTime.UtcNow;

                        // CRÉATION AUTOMATIQUE D’UNE TRANSACTION LIÉE
                        var transaction = new Transaction
                        {
                            UserId = payment.UserId,
                            Titre = "Paiement Stripe",
                            Type = "revenu", // indique un revenu pour l’utilisateur
                            Montant = payment.Amount,
                            Date = DateTime.Now
                        };

                        _db.Transactions.Add(transaction);
                        await _db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Capture des erreurs Stripe (signature invalide, mauvais format, etc.)
                Console.WriteLine($"⚠️ Erreur Webhook Stripe : {ex.Message}");
            }
        }
    }
}
