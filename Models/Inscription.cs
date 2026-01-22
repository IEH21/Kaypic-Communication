using System.ComponentModel.DataAnnotations;

namespace Web3_kaypic.Models
{
    public class Inscription
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le prénom est requis.")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [Display(Name = "Nom")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le courriel est requis.")]
        [EmailAddress(ErrorMessage = "Adresse courriel invalide.")]
        [Display(Name = "Courriel")]
        public string Courriel { get; set; }

        // Ajout numéro de téléphone pour le code de vérification
        [Required(ErrorMessage = "Le numéro de téléphone est requis.")]
        [Display(Name = "Numéro de téléphone")]
        [Phone(ErrorMessage = "Numéro invalide.")]
        public string NumeroTelephone { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        public string MotDePasse { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est requise.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmerMotDePasse { get; set; }
    }
}
