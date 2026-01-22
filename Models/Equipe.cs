namespace Web3_kaypic.Models
{
    public class Equipe
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Sport { get; set; } = string.Empty;
        public string Entraineur { get; set; } = string.Empty;
        public int NbJoueurs { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // Champs supplémentaires
        public int Victoires { get; set; }
        public int Defaites { get; set; }
        public int Nuls { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}