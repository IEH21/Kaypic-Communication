using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web3_kaypic.Models
{
    public class TMessageComment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int comment_id { get; set; }

        [ForeignKey(nameof(Message))]
        public int msg_id { get; set; }
        public TMessage? Message { get; set; }

        [Required]
        [StringLength(255)]
        public string comment_username { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string comment_content { get; set; } = string.Empty;

        public DateTime comment_created_at { get; set; }

        [StringLength(500)] 
        public string? comment_user_profile_image_url { get; set; }
    }
}

