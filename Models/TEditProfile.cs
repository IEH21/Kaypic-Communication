using System.ComponentModel.DataAnnotations;

namespace web3_kaypic.Models
{
    public class TEditProfile
    {
        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Numéro de téléphone invalide")]
        public string? PhoneNumber { get; set; }

        public IFormFile? ProfileImage { get; set; }

        public string? CurrentProfileImageUrl { get; set; }
    }
}