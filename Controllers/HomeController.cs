using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using web3_kaypic.Models;
using Web3_kaypic.Models;
namespace Web3_kaypic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        // ------------------------- PAGE D'ACCUEIL -------------------------
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        // ------------------------- PAGE D'ERREUR -------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
        // ------------------------- INSCRIPTION -------------------------
        [HttpGet]
        public IActionResult Inscription()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Inscription(Inscription model)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Inscription réussie !";
                return RedirectToAction("Connexion");
            }
            return View(model);
        }
        // ------------------------- COMMUNICATION -------------------------
        public IActionResult Communication()
        {
            return View();
        }
        // ------------------------- ÉQUIPES -------------------------
        public IActionResult Equipes()
        {
            // Version static — parfait tant que tu n'utilises pas la BD
            var equipes = new List<Equipe>
            {
                new Equipe
                {
                    Id = 1, Nom = "Les Lions du Soccer", Sport = "Soccer",
                    Entraineur = "Mme Dupont", NbJoueurs = 15, ImageUrl = "/images/soccer.jpg",
                    Victoires = 10, Defaites = 2, Nuls = 3,
                    Description = "Une équipe dynamique et offensive."
                },
                new Equipe
                {
                    Id = 2, Nom = "Les Aigles du Basketball", Sport = "Basketball",
                    Entraineur = "M. Tremblay", NbJoueurs = 12, ImageUrl = "/images/basket.jpg",
                    Victoires = 8, Defaites = 4, Nuls = 1,
                    Description = "Une équipe aérienne et rapide."
                },
                new Equipe
                {
                    Id = 3, Nom = "Les Titans du Volleyball", Sport = "Volleyball",
                    Entraineur = "Mme Nguyen", NbJoueurs = 10, ImageUrl = "/images/volley.jpg",
                    Victoires = 12, Defaites = 1, Nuls = 2,
                    Description = "Une équipe puissante au filet."
                },
                new Equipe
                {
                    Id = 4, Nom = "Les Requins du Hockey", Sport = "Hockey",
                    Entraineur = "M. Gagnon", NbJoueurs = 18, ImageUrl = "/images/hockey.jpg",
                    Victoires = 9, Defaites = 5, Nuls = 2,
                    Description = "Une équipe agressive et tactique."
                },
                new Equipe
                {
                    Id = 5, Nom = "Les Cougars de l’Athlétisme", Sport = "Athlétisme",
                    Entraineur = "Mme Lavoie", NbJoueurs = 20, ImageUrl = "/images/athletisme.jpg",
                    Victoires = 15, Defaites = 0, Nuls = 0,
                    Description = "Des champions de vitesse et d’endurance."
                }
            };
            return View(equipes);
        }
        // ------------------------- ACTUALITÉ -------------------------
        private static List<Post> _posts = new()
        {
            new Post
            {
                Id = 1, UserName = "User123", Content = "Hi everyone",
                CreatedAt = DateTime.Now.AddMinutes(-5),
                UserAvatarUrl = "/image/account.png"
            },
            new Post
            {
                Id = 2, UserName = "User5", Content = "When is next practice?",
                CreatedAt = DateTime.Now.AddDays(-1),
                UserAvatarUrl = "/image/account.png"
            }
        };
        public IActionResult Actualite()
        {
            return View(_posts);
        }
        [HttpPost]
        public IActionResult Create(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                var post = new Post
                {
                    Id = _posts.Count + 1,
                    UserName = "UserActuel",
                    Content = content,
                    CreatedAt = DateTime.Now,
                    UserAvatarUrl = "/image/account.png"
                };
                _posts.Insert(0, post);
            }
            return RedirectToAction("Actualite");
        }
    }
}