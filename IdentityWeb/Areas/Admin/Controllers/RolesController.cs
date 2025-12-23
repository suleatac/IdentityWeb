using IdentityWeb.Areas.Admin.Models;
using IdentityWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWeb.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : Controller
    {


        // GET: Admin/Roles
        public async Task<IActionResult> Index()
        {
            var roleList = await roleManager.Roles.ToListAsync();
            var roleViewModelList = roleList.Select(x => new RoleViewModel() {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return View(roleViewModelList);
        }


        // GET: Admin/Roles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var vm = new RoleViewModel {
                Id = role.Id,
                Name = role.Name
            };

            return View(vm);
        }
       
        // GET: Admin/Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleCreateViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new AppRole {
                    Name = roleViewModel.Name,
                };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }




                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(roleViewModel);
        }

        // Yani hem admin hem de role-action rollerine sahip kullanıcılar bu aksiyona erişebilir.
        [Authorize(Roles = "Admin")]
        // GET: Admin/Roles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var vm = new RoleUpdateViewModel {
                Id = role.Id,
                Name = role.Name ?? string.Empty
            };

            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        // POST: Admin/Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                return NotFound();
            }

            role.Name = model.Name;

            var result = await roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        // GET: Admin/Roles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var vm = new RoleViewModel {
                Id = role.Id,
                Name = role.Name
            };

            return View(vm);
        }


        // POST: Admin/Roles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(RoleViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                return NotFound();
            }

            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.Name ??= role.Name;
            return View(model);
        }


        public async Task<IActionResult> RoleAssignToUser(string UserId) {             
            var currentUser = await userManager.FindByIdAsync(UserId);
            ViewBag.UserId = UserId;
            var roles= await roleManager.Roles.ToListAsync();

            var model = new List<RoleAssignViewModel>();
            foreach (var role in roles)
            {
                var roleAssignViewModel = new RoleAssignViewModel();
                roleAssignViewModel.Id = role.Id;
                roleAssignViewModel.Name = role.Name!;
                roleAssignViewModel.Exist = await userManager.IsInRoleAsync(currentUser!, role.Name!);
                model.Add(roleAssignViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleAssignToUser(string UserId,List<RoleAssignViewModel> requestList)
        {
            var currentUser = await userManager.FindByIdAsync(UserId);
            foreach (var role in requestList)
            {
                if (role.Exist)
                {
                    await userManager.AddToRoleAsync(currentUser!, role.Name);

                }
                else
                {
                    await userManager.RemoveFromRoleAsync(currentUser!, role.Name);
                }
            }
            return RedirectToAction("UserList", "Home");
        }
    }
}
