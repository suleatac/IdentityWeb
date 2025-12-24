using IdentityWeb.Models;
using IdentityWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Security.Claims;


namespace IdentityWeb.Controllers
{
    [Authorize]
    public class MemberController(
        SignInManager<AppUser> signInManager, 
        UserManager<AppUser> userManager,
        IFileProvider fileProvider
        ) : Controller
    {
        public async Task LogOut()
        {
            await signInManager.SignOutAsync();

     
        }
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            if (currentUser == null) 
            { 
                throw new Exception(string.Format("User {0} not found.", User.Identity!.Name!));
            }
            var userViewModel = new UserGeneralViewModel
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture
            };
            return View(userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Claims()
        {
            var userClaimList = User.Claims
                .Select(x => new ClaimViewModel {
                    Issuer = x.Issuer,
                    Type = x.Type,
                    Value = x.Value
                })
                .ToList();
            return View(userClaimList);
        }

        [Authorize(Policy ="AnkaraPolicy")]
        public IActionResult AnkaraPage()
        {

            return View();
        }
        [Authorize(Policy = "ExchangeExpirePolicy")]
        public IActionResult ExchangePage()
        {

            return View();
        }
        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult ViolencePage()
        {

            return View();
        }
        public IActionResult PasswordChange()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "kullanıcı bulunamadı");
                return View(model);

            }
            var checkOldPassword = await userManager.CheckPasswordAsync(currentUser, model.CurrentPassword);
            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifrenizi yanlış girdiniz.");
                return View(model);

            }

            var changePasswordResult = await userManager.ChangePasswordAsync(currentUser, model.CurrentPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await userManager.UpdateSecurityStampAsync(currentUser);
            await signInManager.SignOutAsync();
            await signInManager.PasswordSignInAsync(currentUser, model.NewPassword, true, false);






            TempData["SuccessMessage"] = "Şifre başarıyla değiştirildi.";
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));
            var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var userEditViewModel = new UserEditViewModel() {
                UserName = currentUser!.UserName!,
                Email=currentUser.Email!,
                Phone=currentUser.PhoneNumber!,
                BirthDate=currentUser.BirthDate,
                City=currentUser.City,
                Gender=currentUser.Gender

            };

            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel userEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userEditViewModel);
            }

         






            var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
                return View(userEditViewModel);
            }

            currentUser.UserName = userEditViewModel.UserName;
            currentUser.Email = userEditViewModel.Email;
            currentUser.PhoneNumber = userEditViewModel.Phone;
            currentUser.BirthDate = userEditViewModel.BirthDate;
            currentUser.City = userEditViewModel.City;
            currentUser.Gender = userEditViewModel.Gender;



            if (userEditViewModel.Picture != null && userEditViewModel.Picture.Length > 0)
            {
                var wwwrootPath = fileProvider.GetDirectoryContents("wwwroot");
                if (wwwrootPath == null)
                {
                    ModelState.AddModelError(string.Empty, "Dosya yolu bulunamadı.");
                    return View(userEditViewModel);
                }
                string randomFileName = Guid.NewGuid().ToString() + Path.GetExtension(userEditViewModel.Picture.FileName);
                var newPicturePath= Path.Combine(wwwrootPath.First(x=>x.Name=="userpictures").PhysicalPath!, randomFileName);
                using (var stream = new FileStream(newPicturePath, FileMode.Create))
                {
                    await userEditViewModel.Picture.CopyToAsync(stream);
                }




                currentUser.Picture = randomFileName;
            }














            var result = await userManager.UpdateAsync(currentUser);
            if (result.Succeeded)
            {
                await userManager.UpdateSecurityStampAsync(currentUser);
                //await signInManager.RefreshSignInAsync(currentUser);
                await signInManager.SignOutAsync();
                await signInManager.SignInAsync(currentUser, true);
                if (currentUser.BirthDate.HasValue)
                {
                    await signInManager.SignInWithClaimsAsync(currentUser, true, new[] { new Claim("birthdate", currentUser.BirthDate.Value.ToString()) });
                }

                TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(userEditViewModel);
        }













    }
}
