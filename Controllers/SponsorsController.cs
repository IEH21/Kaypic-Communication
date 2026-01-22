using Microsoft.AspNetCore.Mvc;
using Web3_kaypic.Models;

namespace Web3_kaypic.Controllers
{
    public class SponsorsController : Controller
    {
        // Méthode Index : affiche la liste des sponsors (optionnellement filtrée par équipe)
        public IActionResult Index(int? equipeId)
        {
            // Liste statique de sponsors (pas encore de base de données)
            var sponsors = new List<Sponsor>
            {
                new Sponsor
                {
                    Id = 1,
                    Nom = "Decathlon",
                    Type = "Or",
                    LogoUrl = "/images/decathlon.png",
                    SiteWeb = "https://www.decathlon.ca/fr/content/affiliation",
                    Description = "Fournisseur officiel d’équipements sportifs.",
                    EquipesIds = new List<int>{ 1, 2 }
                },
                new Sponsor
                {
                    Id = 2,
                    Nom = "PowerDrink",
                    Type = "Argent",
                    LogoUrl = "/images/powerdrink.png",
                    SiteWeb = "https://www.powerade.com/partners",
                    Description = "Boissons énergétiques naturelles pour athlètes.",
                    EquipesIds = new List<int>{ 1, 5 }
                },
                new Sponsor
                {
                    Id = 3,
                    Nom = "TechSport",
                    Type = "Bronze",
                    LogoUrl = "/images/techsport.png",
                    SiteWeb = "https://www.grcupseries.com/news/102/techsport-racing-announces-2025-driver-lineup",
                    Description = "Analyse de performance et technologie sportive.",
                    EquipesIds = new List<int>{ 2, 3 }
                },
                new Sponsor
                {
                    Id = 4,
                    Nom = "Clinique Santé+",
                    Type = "Or",
                    LogoUrl = "/images/santeplus.png",
                    SiteWeb = "https://partenairesante.ca/",
                    Description = "Physiothérapie et accompagnement médical sportif.",
                    EquipesIds = new List<int>{ 3, 4, 5 }
                }
            };

            // Si un ID d’équipe est fourni dans l’URL (/Sponsors?equipeId=1)
            //     alors on filtre uniquement les sponsors liés à cette équipe
            if (equipeId.HasValue)
            {
                sponsors = sponsors
                    .Where(s => s.EquipesIds.Contains(equipeId.Value))
                    .ToList();

                // On passe l'ID à la vue pour afficher un titre personnalisé
                ViewBag.EquipeId = equipeId.Value;
            }

            // On retourne la liste (filtrée ou complète) à la vue
            return View(sponsors);
        }
    }
}
