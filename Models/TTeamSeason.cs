using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using web3_kaypic.Models;

namespace Web3_kaypic.Models
{
    public class TTeamSeason
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ts_id { get; set; }

        [Required, StringLength(2)]
        public string ts_status { get; set; } = "ac"; // ac/in

        [Required, StringLength(64)]
        public string ts_chat_key { get; set; } = string.Empty; // UNIQUE

        [Required, StringLength(120)]
        public string ts_name { get; set; } = string.Empty;

        public DateTime created_at { get; set; }

        public List<TMessagingPersona> Personas { get; set; } = new();
        public List<TMessagingChat> Chats { get; set; } = new();
        public List<TNews> News { get; set; } = new();
        public List<TMessagingMedia> Medias { get; set; } = new();
        public List<TMessagingChatPersona> ChatPersonas { get; set; } = new();
        public List<TMessagingChatPersonaMessage> ChatMessages { get; set; } = new();
    }
}
