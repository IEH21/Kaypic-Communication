using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web3_kaypic.Models;

namespace Web3_kaypic.Models
{
    public class TMessagingChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mc_id { get; set; }

        [ForeignKey(nameof(TeamSeason))]
        public int ts_id { get; set; }
        public TTeamSeason? TeamSeason { get; set; }

        [Required, StringLength(10)]
        public string mc_status { get; set; } = "active";

        [StringLength(160)]
        public string? mc_title { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public int created_by_mp_id { get; set; }
        public TMessagingPersona? CreatedBy { get; set; }

        public DateTime created_at { get; set; }

        public List<TMessagingChatPersona> ChatPersonas { get; set; } = new();
        public List<TMessagingMedia> Medias { get; set; } = new();

        public List<TMessagingChatPersonaMessage> Messages { get; set; } = new();
    }
}
