using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextHome.Realty.Application.Common.Interfaces;
using NextHome.Realty.Application.Common.Utility;
using NextHome.Realty.Domain.Entities;
using NextHome.Realty.Persistence.Data;
using NextHome.Realty.Web.ViewModels;

namespace NextHome.Realty.Web.Controllers
{
    public class AccountController(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager)
        : Controller
    {
        public async Task<IActionResult> Login(string? returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            var loginVM = new LoginVM
            {
              RedirectUrl = returnUrl,
            };
            return View(loginVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVm)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager
                    .PasswordSignInAsync(
                        loginVm.Email, loginVm.Password,
                        loginVm.RememberMe, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVm.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(loginVm.RedirectUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");

                }
            }
            return View(loginVm);
        }

        public async Task<IActionResult> Register(string? returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            if (!await roleManager.RoleExistsAsync(SD.RoleType.Admin.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.RoleType.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(SD.RoleType.Customer.ToString()));
            }

            var registerMV = new RegisterVM
            {
                RoleListItems = roleManager.Roles.Select(x =>
                    new SelectListItem { Text = x.Name, Value = x.Name }
                ).ToList(),
                RedirectUrl = returnUrl
            };

            return View(registerMV);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVm)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = registerVm.Name,
                    Email = registerVm.Email,
                    PhoneNumber = registerVm.PhoneNumber,
                    NormalizedEmail = registerVm.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerVm.Email,
                    CreatedAt = DateTime.UtcNow
                };
                var result = await userManager.CreateAsync(user, registerVm.Password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVm.Role))
                    {
                        await userManager.AddToRoleAsync(user, registerVm.Role);
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, SD.RoleType.Customer.ToString());
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);
                    if (string.IsNullOrEmpty(registerVm.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(registerVm.RedirectUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            registerVm.RoleListItems = roleManager.Roles.Select(x =>
                new SelectListItem { Text = x.Name, Value = x.Name }
            ).ToList();


            return View(registerVm);
        }

   
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
