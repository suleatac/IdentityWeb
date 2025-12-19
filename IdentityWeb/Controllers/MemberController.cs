using IdentityWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWeb.Controllers
{
    [Authorize]
    public class MemberController(SignInManager<AppUser> signInManager) : Controller
    {
        public async Task LogOut()
        {
            await signInManager.SignOutAsync();

     
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
