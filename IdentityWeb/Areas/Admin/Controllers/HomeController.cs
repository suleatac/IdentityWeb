using IdentityWeb.Areas.Admin.Models;
using IdentityWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController(UserManager<AppUser> userManager) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UserList()
        {
            var userList = await userManager.Users.ToListAsync();
            var userViewModelList = userList.Select(x => new UserViewModel() {
                UserId = x.Id,
                UserName = x.UserName,
                Email = x.Email,

            }).ToList();
            return View(userViewModelList);
        }
    }
}
