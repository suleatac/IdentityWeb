using Microsoft.AspNetCore.Identity;

namespace IdentityWeb.Models
{
    public class AppUser:IdentityUser
    {
        public string? City { get; set; }
    }
}
