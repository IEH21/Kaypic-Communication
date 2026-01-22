namespace Web3_kaypic.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserAvatarUrl { get; set; }
    }
}
