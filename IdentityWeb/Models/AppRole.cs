using Microsoft.AspNetCore.Identity;

namespace IdentityWeb.Models
{
    public class AppRole:IdentityRole
    {
        public string? RoleSeviyesi { get; set; }
    }
}
