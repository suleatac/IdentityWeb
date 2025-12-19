using System.Diagnostics;
using IdentityWeb.Models;
using IdentityWeb.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityWeb.Controllers
{
    public class HomeController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
        public  IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {


            if (!ModelState.IsValid) 
            {
                return View();

            }
            var identityResult= await userManager.CreateAsync(new AppUser { UserName=signUpViewModel.UserName,PhoneNumber=signUpViewModel.Phone,Email=signUpViewModel.Email}, signUpViewModel.Password);
           
            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayýt iþlemi baþarýyla gerçekleþmiþtir";
                return View();
            }

            foreach (IdentityError item in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
            }




            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string? returnUrl=null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var hasUser = await userManager.FindByEmailAsync(signInViewModel.Email);
            if (hasUser == null) 
            {
                ModelState.AddModelError(string.Empty, "Email veya þifre yanlýþ");
                return View();

            }

            var result = await signInManager.PasswordSignInAsync(hasUser,signInViewModel.Password,signInViewModel.RememberMe,true);
            if (result.Succeeded)
            {
               
                return Redirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                var failedAccessCount = await userManager.GetAccessFailedCountAsync(hasUser);
                ModelState.AddModelError(string.Empty, $"Baþarýsýz giriþ sayýsý {failedAccessCount}");
                TempData["ErrorMessage"] = "3dk Boyunca giriþ yapamazsýnýz. Çok fazla deneme yaptýnýz.";
                return View();
            }

            ModelState.AddModelError(string.Empty, "Email veya þifre yanlýþ");
            
            return View();
        }

    
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
