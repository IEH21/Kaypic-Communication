using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web3_kaypic.Models;

namespace web3_kaypic.Models
{

    public class TMessagingChatPersonaMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long mcpm_id { get; set; }

        [ForeignKey(nameof(ChatPersona))]
        public int mcp_id { get; set; }
        public TMessagingChatPersona? ChatPersona { get; set; }

        [ForeignKey(nameof(Chat))]
        public int mc_id { get; set; }
        public TMessagingChat? Chat { get; set; }

        [ForeignKey(nameof(TeamSeason))]
        public int ts_id { get; set; }
        public TTeamSeason? TeamSeason { get; set; }

        [Required]
        public string mcpm_message { get; set; } = string.Empty;

        [ForeignKey(nameof(ReplyToMessage))]
        public long? reply_to_id { get; set; }
        public TMessagingChatPersonaMessage? ReplyToMessage { get; set; }

        [Required]
        public bool is_deleted { get; set; } = false;

        public DateTime? edited_at { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public List<TMessagingChatPersonaMessage> Replies { get; set; } = new();
    }


}