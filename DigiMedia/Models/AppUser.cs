using Microsoft.AspNetCore.Identity;

namespace DigiMedia.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; } = string.Empty;
    }
}
