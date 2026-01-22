using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web3_kaypic.Models
{
    [Table("Matches")]
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Equipe { get; set; } = string.Empty;

        [Required]
        public DateTime DateMatch { get; set; }

        [Required]
        public string Lieu { get; set; } = string.Empty;

        public string Equipement { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailCoach { get; set; } = string.Empty;

        // Pour éviter les doublons d’envoi
        public bool Rappel24hEnvoye { get; set; } = false;
        public bool RappelJourJEnvoye { get; set; } = false;
    }
}
