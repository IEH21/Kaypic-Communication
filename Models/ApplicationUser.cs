using Microsoft.AspNetCore.Identity;

namespace web3_kaypic.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
