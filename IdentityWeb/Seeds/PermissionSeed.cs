using IdentityWeb.Models;
using IdentityWeb.Permissions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityWeb.Seeds
{
    public class PermissionSeed
    {
        public  static async Task SeedAsync(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
            if (!hasBasicRole)
            {
                var basicRole = new AppRole() {
                    Name = "BasicRole"
                };
                await roleManager.CreateAsync(basicRole);

                var savedBasicRole = await roleManager.FindByNameAsync("BasicRole");
                if (savedBasicRole != null)
                {
                    await roleManager.AddClaimAsync(savedBasicRole, new Claim("Permission", PermissionRoot.Stock.Read));
                    await roleManager.AddClaimAsync(savedBasicRole, new Claim("Permission", PermissionRoot.Order.Read));
                    await roleManager.AddClaimAsync(savedBasicRole, new Claim("Permission", PermissionRoot.Stock.Create));




                }



            }
        }









    }
}
