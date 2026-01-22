using System.Collections.Generic;

namespace Web3_kaypic.Models
{
    // Classe représentant un sponsor
    public class Sponsor
    {
        // Identifiant unique du sponsor
        public int Id { get; set; }

        // Nom du sponsor (ex: Decathlon)
        public string Nom { get; set; } = string.Empty;

        // Type de partenariat (ex: Or, Argent, Bronze)
        public string Type { get; set; } = string.Empty;

        // URL du logo du sponsor (stocké dans wwwroot/images)
        public string LogoUrl { get; set; } = string.Empty;

        // URL du site web officiel du sponsor
        public string SiteWeb { get; set; } = string.Empty;

        // Brève description du sponsor
        public string Description { get; set; } = string.Empty;

        // Liste des identifiants des équipes sponsorisées
        public List<int> EquipesIds { get; set; } = new();
    }
}