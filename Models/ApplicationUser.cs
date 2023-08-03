using Microsoft.AspNetCore.Identity;

namespace MediaManageAPI.Models
{
    public class ApplicationUser : IdentityUser{
        public string? YoutubeRefreshToken { get; set; } = string.Empty;
    }
}