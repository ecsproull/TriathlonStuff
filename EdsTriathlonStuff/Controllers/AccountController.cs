using EdsTriathlonStuff.App_Code;
using EdsTriathlonStuff.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdsTriathlonStuff.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private static string EmailToConfirm = string.Empty;
        private static string UserIdToConfirm = string.Empty;

        public AccountController(UserManager<AppUser> userMgr,
                                 SignInManager<AppUser> signinMgr,
                                 RoleManager<IdentityRole> roleMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            roleManager = roleMgr;
        }

        public async Task<IActionResult> Login(string returnUrl)
        {
            int userCount = await userManager.Users.CountAsync();
            if (userCount == 0)
            {
                AppUser user = new AppUser
                {
                    UserName = "Admin",
                    Email = "admin@admin.com"
                };

                string roleName = "Administrator";
                IdentityResult result = await userManager.CreateAsync(user, "Admin1234#");
                if (result.Succeeded)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (result.Succeeded)
                    {
                        result = await userManager.AddToRoleAsync(user, roleName);
                        if (result.Succeeded)
                        {
                            // Add some poi types.
                        }
                    }
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(new LoginViewModel(User)
            {
                //LoginButtonClass = User.Identity.IsAuthenticated ? "loggedin-button" : "loggedout-button"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            //Just curious if this is true.
                        }

                        if (await userManager.IsInRoleAsync(user, "Administrator"))
                        {
                            return RedirectToAction("Index", "Admin");
                        }

                        return Redirect(returnUrl ?? "/");
                    }
                    else
                    {
                        return Redirect($"/Account/Login");
                    }
                }

                ModelState.AddModelError(nameof(LoginViewModel.Email), " Invalid user or password");
            }

            return View(details);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private static CreateUserViewModel PreErrorData = null;
        public ViewResult CreateUser()
        {
            CreateUserViewModel vm = new CreateUserViewModel(User)
            {
                RoleNames = new List<string>(),
                SelectedRoles = new List<bool>()
            };

            if (PreErrorData != null)
            {
                vm.Name = PreErrorData.Name;
                vm.Email = PreErrorData.Email;
                vm.Password = PreErrorData.Password;

                for (int i = 0; i < vm.SelectedRoles.Count; i++)
                {
                    vm.SelectedRoles[i] = PreErrorData.SelectedRoles[i];
                }

                PreErrorData = null;
            }

            vm.AllowRoleSelection = false;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                if (VerifyEmailDoesntExist(user.Email))
                {
                    if (VerifyUserNameDoesntExist(user.UserName))
                    {
                        IdentityResult result = await userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "Coach");
                            EmailToConfirm = user.Email;
                            UserIdToConfirm = user.Id;
                            //return Redirect($"/Account/ConfirmSendEmail");
                        }
                        else
                        {
                            foreach (IdentityError error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Name", "UserName is already in use.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email address is already in use.");
                }
            }
            model.AllowRoleSelection = false;
            return View(model);
        }

        public bool VerifyEmailDoesntExist(string email)
        {
            return (from x in userManager.Users
                    where x.Email == email
                    select x.Id).FirstOrDefault() == null;
        }
        public bool VerifyUserNameDoesntExist(string userName)
        {
            return (from x in userManager.Users
                    where x.UserName == userName
                    select x.Id).FirstOrDefault() == null;
        }
    }
}
