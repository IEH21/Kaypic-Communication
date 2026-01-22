using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web3_kaypic.Models;

namespace Web3_kaypic.Models
{
    public class TMessagingChatPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mcp_id { get; set; }


        [ForeignKey(nameof(Chat))]
        public int mc_id { get; set; }
        public TMessagingChat? Chat { get; set; }

        [ForeignKey(nameof(TeamSeason))]
        public int ts_id { get; set; }
        public TTeamSeason? TeamSeason { get; set; }

        [ForeignKey(nameof(Persona))]
        public int mp_id { get; set; }
        public TMessagingPersona? Persona { get; set; }

        [Required, StringLength(10)]
        public string mcp_status { get; set; } = "ac";

        public DateTime added_at { get; set; }

        public List<TMessagingChatPersonaMessage> Messages { get; set; } = new();
    }
}
