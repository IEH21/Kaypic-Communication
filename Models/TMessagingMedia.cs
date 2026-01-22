using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web3_kaypic.Models;

public class TMessagingMedia
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int mcm_id { get; set; }

    [ForeignKey(nameof(MessagingChat))]
    public int mc_id { get; set; }
    public TMessagingChat? MessagingChat { get; set; }

    [ForeignKey(nameof(TeamSeason))]
    public int ts_id { get; set; }
    public TTeamSeason? TeamSeason { get; set; }

    [Required, StringLength(1)]
    public string mcm_media_category { get; set; } = "I"; // I = image, V = vidéo, D = document


    [Required, StringLength(400)]
    public string mcm_url { get; set; } = string.Empty;

    [ForeignKey(nameof(Creator))]
    public int created_by_mp_id { get; set; }
    public TMessagingPersona? Creator { get; set; }

    public DateTime created_at { get; set; } = DateTime.UtcNow;

    // --- Métadonnées supplémentaires ---
    [StringLength(255)]
    public string mcm_filename { get; set; } = string.Empty;

    public long mcm_size { get; set; }

}
