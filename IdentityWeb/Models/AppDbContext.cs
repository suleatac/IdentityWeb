using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityWeb.ViewModels;
using IdentityWeb.Areas.Admin.Models;

namespace IdentityWeb.Models
{
    public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {}
      
    


    }
}
