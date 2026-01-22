using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web3_kaypic.Models
{
    public class TPlayer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int player_id { get; set; }

        [Required, MaxLength(80)]
        public string player_fname { get; set; }

        [Required, MaxLength(80)]
        public string player_lname { get; set; }

        public string? statut { get; set; }

        [MaxLength(120)]
        public string? player_email { get; set; }

        [MaxLength(80)]
        public string? player_guardian_fname { get; set; }

        [MaxLength(80)]
        public string? player_guardian_lname { get; set; }

        [MaxLength(120)]
        public string? player_guardian_email { get; set; }

        public DateTime created_at { get; set; }

        [ForeignKey(nameof(SaisonEquipe))]
        public int IdSaison { get; set; }
        public TTeamSeason? SaisonEquipe { get; set; }

        public TMessagingPersona? PersonneMessagerie { get; set; } // link by email at runtime
    }
}
