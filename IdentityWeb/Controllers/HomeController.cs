using System.Diagnostics;
using System.Security.Claims;
using IdentityWeb.Models;
using IdentityWeb.Services;
using IdentityWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityWeb.Controllers
{
    public class HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult SignUp()
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
            var identityResult = await userManager.CreateAsync(new AppUser { UserName = signUpViewModel.UserName, PhoneNumber = signUpViewModel.Phone, Email = signUpViewModel.Email }, signUpViewModel.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError item in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View();

            }

            var currentUser = await userManager.FindByEmailAsync(signUpViewModel.Email);
            var exchangeClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());
            var claimResult = await userManager.AddClaimAsync(currentUser!, exchangeClaim);

            if (!claimResult.Succeeded)
            {
                foreach (var item in claimResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View();
            }
            TempData["SuccessMessage"] = "Üyelik kayýt iþlemi baþarýyla gerçekleþmiþtir";
            return View();
        }




        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInViewModel, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var hasUser = await userManager.FindByEmailAsync(signInViewModel.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya þifre yanlýþ");
                return View();

            }

            var passwordCheck = await userManager.CheckPasswordAsync(hasUser, signInViewModel.Password);
            if (!passwordCheck)
            {
                ModelState.AddModelError(string.Empty, "Email veya þifre yanlýþ");
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(hasUser, signInViewModel.Password, signInViewModel.RememberMe, true);
            if (result.IsLockedOut)
            {
                var failedAccessCount = await userManager.GetAccessFailedCountAsync(hasUser);
                ModelState.AddModelError(string.Empty, $"Baþarýsýz giriþ sayýsý {failedAccessCount}");
                TempData["ErrorMessage"] = "3dk Boyunca giriþ yapamazsýnýz. Çok fazla deneme yaptýnýz.";
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email veya þifre yanlýþ");
                ModelState.AddModelError(string.Empty, $"Baþarýsýz giriþ denemesi={await userManager.GetAccessFailedCountAsync(hasUser)}");
                return View();
             
            }

            if (hasUser.BirthDate.HasValue)
            {
                await signInManager.SignInWithClaimsAsync(hasUser, signInViewModel.RememberMe, new[] {new Claim("birthdate", hasUser.BirthDate.Value.ToString())});
            }


           



            return Redirect(returnUrl!);

        }




        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            //link
            //Uygulama þifresi: qbrt pcxg nnxw uvmv
            //https://localhost:7006?userId=12213&token=aadsdfgfgsfgs

            var hasUser = await userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu mail adresine sahip kullanýcý bulunamamýþtýr.");
                return View();

            }
            string passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwordResetLink = Url.Action("SetNewPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme, "localhost:7199");

            await emailService.SendResetEmail(passwordResetLink!, resetPasswordViewModel.Email);



            TempData["SuccessMessage"] = "Þifre yenileme linki mail adresinize gönderilmiþtir.";





            return RedirectToAction(nameof(ResetPassword));
        }



        public IActionResult SetNewPassword(string userId, string token)
        {
            var model = new SetNewPasswordModel {
                UserId = userId,
                Token = token
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SetNewPassword(SetNewPasswordModel setNewPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(setNewPasswordModel);
            }

            var hasUser = await userManager.FindByIdAsync(setNewPasswordModel.UserId);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu kullanýcý bulunamamýþtýr.");
                return View(setNewPasswordModel);
            }

            var result = await userManager.ResetPasswordAsync(hasUser, setNewPasswordModel.Token, setNewPasswordModel.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }

                return View(setNewPasswordModel);
            }

            TempData["SuccessMessage"] = "Þifre baþarýyla güncellenmiþtir.";
            return RedirectToAction(nameof(SignIn));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
