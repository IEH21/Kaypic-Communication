using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web3_kaypic.Models
{

    public class TMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int msg_id { get; set; }

        [Required]
        [StringLength(255)]
        public string msg_username { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string msg_content { get; set; } = string.Empty;

        public DateTime msg_created_at { get; set; }

        [StringLength(500)]
        public string? msg_image_url { get; set; }

        //Pour Like
        public int msg_likes_count { get; set; } = 0;

        //Pour commentaire
        public virtual ICollection<TMessageComment> Comments { get; set; } = new List<TMessageComment>();


        [StringLength(500)] 
        public string? msg_user_profile_image_url { get; set; }



    }
}

