using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web3_kaypic.Models
{
    public class TMessagingPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mp_id { get; set; }

        [ForeignKey(nameof(TeamSeason))]
        public int ts_id { get; set; }
        public TTeamSeason? TeamSeason { get; set; }

        [Required, StringLength(2)]
        public string mp_status { get; set; } = "ac";

        [Required, StringLength(12)]
        public string mp_category { get; set; } = "participant"; // participant | parent | teammanager

        [Required, StringLength(120)]
        [EmailAddress]
        public string mp_email { get; set; } = string.Empty; // UNIQUE

        [Required, StringLength(80)]
        public string mp_lname { get; set; } = string.Empty;

        [Required, StringLength(80)]
        public string mp_fname { get; set; } = string.Empty;

        public DateTime created_at { get; set; }

        public List<TMessagingChat> CreatedChats { get; set; } = new();
        public List<TNews> AuthoredNews { get; set; } = new();
        public List<TMessagingChatPersona> ChatPersonas { get; set; } = new();
        public List<TMessagingMedia> Medias { get; set; } = new();
    }
}
