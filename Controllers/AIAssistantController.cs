using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Web3_kaypic.Controllers
{
    public class AIAssistantController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AIAssistantController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            try
            {
                // Pour l'instant, retourner une réponse simple sans API
                var response = GenerateSimpleResponse(request.Message);

                return Json(new
                {
                    success = true,
                    message = response
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = "Une erreur s'est produite. Veuillez réessayer."
                });
            }
        }

        private string GenerateSimpleResponse(string userMessage)
        {
            var message = userMessage.ToLower();

            // Réponses basiques selon les mots-clés
            if (message.Contains("équipe") || message.Contains("team"))
            {
                return "Pour créer une nouvelle équipe, allez dans le menu principal et cliquez sur 'Gestion des équipes'. Vous pourrez y ajouter des membres, définir les rôles et configurer les paramètres de votre équipe.";
            }
            else if (message.Contains("message") || message.Contains("chat"))
            {
                return "Pour envoyer un message, cliquez sur l'icône 💬 dans le menu latéral, sélectionnez une conversation existante ou créez-en une nouvelle. Vous pouvez aussi envoyer des fichiers et des images !";
            }
            else if (message.Contains("calendrier") || message.Contains("calendar"))
            {
                return "Le calendrier vous permet de gérer vos événements et rendez-vous. Cliquez sur 📅 dans le menu pour accéder à votre calendrier. Vous pouvez y créer des événements, inviter des participants et configurer des rappels.";
            }
            else if (message.Contains("annonce"))
            {
                return "Pour publier une annonce, accédez à la section Annonces via le menu Annonce. Vous pouvez y créer des annonces importantes qui seront visibles par tous les membres de votre équipe.";
            }
            else if (message.Contains("notification"))
            {
                return "Gérez vos notifications en cliquant sur l'icône 🔔. Vous y trouverez toutes vos notifications récentes et pourrez configurer vos préférences de notification.";
            }
            else if (message.Contains("aide") || message.Contains("help") || message.Contains("bonjour") || message.Contains("hello"))
            {
                return "Bonjour ! Je suis votre assistant. Je peux vous aider avec :\n\n• Créer et gérer des équipes\n• Envoyer des messages\n• Utiliser le calendrier\n• Publier des annonces\n• Gérer vos notifications\n\nPosez-moi une question sur l'un de ces sujets !";
            }
            else
            {
                return "Je suis là pour vous aider ! Voici ce que je peux faire :\n\n• Vous guider dans l'utilisation de l'application\n• Expliquer les fonctionnalités disponibles\n• Répondre à vos questions\n\nN'hésitez pas à me poser une question spécifique !";
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; }
        public List<ChatMessage> History { get; set; } = new();
    }

    public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
