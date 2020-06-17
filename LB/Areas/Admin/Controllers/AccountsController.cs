using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LB.Models;
using LB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Root")]
    public class AccountsController : Controller
    {
        private readonly Context context;
        private readonly UserManager<AppUser> userManager;

        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private string[] RoleNames = { "Admin", "Editor", "Manager", "Root" };

        public AccountsController(Context _context, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            //var accounts = GetAccounts();
            var accounts = context.AdminUsers.Include(x => x.AppUser).Where(x => x.AppUserId != null);
            return View(await PaginatedList<AdminUser>.CreateAsync(accounts, 1, 10));
        }

        public async Task<IActionResult> Create()
        {
            await CreateRoles();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUser user)
        {
            if (ModelState.IsValid)
            {
                user.AdminUser = new AdminUser
                {
                    LastLogin = DateTime.Now
                };

                if (user.IsPassword)
                {
                    user.EmailConfirmed = true;
                    var result = await userManager.CreateAsync(user, user.Password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                        //await SignInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.Response = Utils.GetSessionError(this.HttpContext);
            ViewBag.Roles = await roleManager.Roles.ToListAsync();
            var account =  await context.AdminUsers.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.AppUserId == id);
            return View(account);
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public async Task<IActionResult> EditPost(Guid id)
        {
            var account = await context.AdminUsers.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.AppUserId == id);
            var roles = roleManager.Roles.Select(x => x.Name);
            var userRoles = await userManager.GetRolesAsync(account.AppUser);
            string err = "";

            if (await TryUpdateModelAsync(account))
            {
                foreach (var role in roles)
                {
                    if (account.SelectedRoles.Contains(role))
                    {
                        if (!userRoles.Contains(role))
                        {
                            await userManager.AddToRoleAsync(account.AppUser, role);
                        }
                    }
                    else
                    {
                        if (userRoles.Contains(role))
                        {
                            await userManager.RemoveFromRoleAsync(account.AppUser, role);
                        }
                    }
                }

                if (account.AppUser.IsPassword || string.IsNullOrEmpty(account.AppUser.Password))
                {
                    await userManager.UpdateAsync(account.AppUser);
                    await context.SaveChangesAsync();
                    //Utils.AddSessionError(this.HttpContext, ResponseStatus.Error, "Account Updated");
                    return RedirectToAction(nameof(Edit), new { id });
                }
                else
                {
                    err = "Passwords do not match";
                }
            }
            else
            {
                err = "Cannot Update Account";
            }

            this.AddSessionError(ResponseStatus.Success, "Success");
            ViewBag.Response = Utils.GetSessionError(this.HttpContext);
            ViewBag.Roles = await roleManager.Roles.ToListAsync();

            return View(account);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var account = await userManager.FindByIdAsync(id.ToString());
            var rootUsers = await userManager.GetUsersInRoleAsync("Root");
            if (!(await userManager.IsInRoleAsync(account, "Root") && rootUsers.Count < 2))
            {
                await userManager.DeleteAsync(account);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            return View(user);
        }















/*---------------------------------------- Anonymous ----------------------------------*/


        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            string roles = RoleNames.Count().ToString();
            foreach (var role in RoleNames)
            {
                roles += $"{role} ";
                if ((await roleManager.FindByNameAsync(role)) == null)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole<Guid>
                    {
                        Name = role
                    });
                    if (!result.Succeeded)
                    {
                        return Content($"Error: {string.Join("<br>", result.Errors)}");
                    }
                }
            }

            //return Content(roles);

            if ((await userManager.FindByNameAsync("Admin")) == null)
            {
                var user = new AppUser()
                {
                    Email = "admin@mail.com",
                    Password = "Password@1234",
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Admin",
                    UserName = "admin"
                };

                user.AdminUser = new AdminUser
                {
                    LastLogin = DateTime.Now
                };

                await userManager.CreateAsync(user, user.Password);
                await userManager.AddToRolesAsync(user, new string[] { "Admin", "Root" });
            }

            if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(
                    user, model.Password, model.RememberMe, false
                    );

                if (result.Succeeded)
                {
                    return Redirect("/admin");
                }
                else
                {
                    if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("", "You cannot login to this account. Make sure you have comfirmed your email Address.");
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        //////////////////////////////////////////////////////////////////////////////
        
        public async Task CreateRoles()
        {
            try
            {
                foreach (var role in RoleNames)
                {
                    if (!(await roleManager.RoleExistsAsync(role)))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>()
                        {
                            Name = role
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddSessionError(ResponseStatus.Warning, ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}