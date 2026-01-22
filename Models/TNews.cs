using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web3_kaypic.Models
{
    public class TNews
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int news_id { get; set; }

        [ForeignKey(nameof(TeamSeason))]
        public int ts_id { get; set; }
        public TTeamSeason? TeamSeason { get; set; }

        [Required, StringLength(10)]
        public string news_status { get; set; } = "active";

        [Required, StringLength(160)]
        public string news_title { get; set; } = string.Empty;

        [Required]
        public string news_body { get; set; } = string.Empty;

        public DateTime news_date_posted { get; set; }
        public DateTime? news_date_start { get; set; }
        public DateTime? news_date_end { get; set; }

        [Required, StringLength(1)]
        public string news_media_category { get; set; } = "n"; // i, v, n

        [StringLength(400)]
        public string? news_media_url { get; set; }

        [ForeignKey(nameof(AuthorPersona))]
        public int news_author_mp_id { get; set; }
        public TMessagingPersona? AuthorPersona { get; set; }
    }
}
