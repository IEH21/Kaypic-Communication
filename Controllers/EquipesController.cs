using Microsoft.AspNetCore.Mvc;
using Web3_kaypic.Models;

namespace Web3_kaypic.Controllers
{
    public class EquipesController : Controller
    {
        public IActionResult Index()
        {
            // Données statiques pour test sans base de données
            var equipes = new List<Equipe>
            {
                new Equipe { Id = 1, Nom = "Les Lions du Soccer", Sport = "Soccer", Entraineur = "Mme Dupont", NbJoueurs = 15, ImageUrl = "/images/soccer.jpg", Victoires = 10, Defaites = 2, Nuls = 3, Description = "Une équipe dynamique et offensive." },
                new Equipe { Id = 2, Nom = "Les Aigles du Basketball", Sport = "Basketball", Entraineur = "M. Tremblay", NbJoueurs = 12, ImageUrl = "/images/basket.jpg", Victoires = 8, Defaites = 4, Nuls = 1, Description = "Une équipe aérienne et rapide." },
                new Equipe { Id = 3, Nom = "Les Titans du Volleyball", Sport = "Volleyball", Entraineur = "Mme Nguyen", NbJoueurs = 10, ImageUrl = "/images/volley.jpg", Victoires = 12, Defaites = 1, Nuls = 2, Description = "Une équipe puissante au filet." },
                new Equipe { Id = 4, Nom = "Les Requins du Hockey", Sport = "Hockey", Entraineur = "M. Gagnon", NbJoueurs = 18, ImageUrl = "/images/hockey.jpg", Victoires = 9, Defaites = 5, Nuls = 2, Description = "Une équipe agressive et tactique." },
                new Equipe { Id = 5, Nom = "Les Cougars de l’Athlétisme", Sport = "Athlétisme", Entraineur = "Mme Lavoie", NbJoueurs = 20, ImageUrl = "/images/athletisme.jpg", Victoires = 15, Defaites = 0, Nuls = 0, Description = "Des champions de vitesse et d’endurance." }
            };

            return View(equipes);
        }
    }
}