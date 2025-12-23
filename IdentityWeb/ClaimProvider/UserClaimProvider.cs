using IdentityWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityWeb.ClaimProvider
{
    public class UserClaimProvider(UserManager<AppUser> userManager) : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            //if (principal.Identity is not ClaimsIdentity identity || !identity.IsAuthenticated)
            //{
            //    return principal;
            //}
            var identity = principal.Identity as ClaimsIdentity;
   
            var userName = identity!.Name;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return principal;
            }

            var currentUser = await userManager.FindByNameAsync(userName);
            if (currentUser == null)
            {
                return principal;
            }

            if (!string.IsNullOrWhiteSpace(currentUser.City) &&
                !principal.HasClaim(c => c.Type == "city"))
            {
                identity.AddClaim(new Claim("city", currentUser.City));
            }

            return principal;
        }
    }
}
