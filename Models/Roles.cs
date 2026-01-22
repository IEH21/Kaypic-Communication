namespace web3_kaypic.Models
{
    /*
     [Authorize(Roles = Roles.Admin)]
    public IActionResult Dashboard() => View();

     */
    public class Roles
    {
        public const string Admin = "Admin";
        public const string Coach = "Coach";
        public const string Tuteur = "Tuteur";
        public const string Jeune = "Jeune";
        public const string Visiteur = "Visiteur";

    }
}
