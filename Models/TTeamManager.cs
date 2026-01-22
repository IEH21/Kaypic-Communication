using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web3_kaypic.Models
{
    public class TTeamManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tm_id { get; set; }

        [Required, MaxLength(80)]
        public string tm_fname { get; set; }

        [Required, MaxLength(80)]
        public string tm_lname { get; set; }

        [Required, MaxLength(120)]
        [EmailAddress]
        public string tm_email { get; set; } = string.Empty; // UNIQUE

        public DateTime created_at { get; set; }

        public List<TTeamSeason>? SaisonEquipes { get; set; }
    }
}
