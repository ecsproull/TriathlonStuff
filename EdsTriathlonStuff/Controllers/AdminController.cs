using EdsTriathlonStuff.App_Code;
using EdsTriathlonStuff.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EdsTriathlonStuff.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private RoleManager<IdentityRole> roleManager;

        public AdminController(UserManager<AppUser> usrMgr,
            IUserValidator<AppUser> userValid,
            IPasswordValidator<AppUser> passValid,
            IPasswordHasher<AppUser> passwordHash,
            RoleManager<IdentityRole> roleMgr)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
            roleManager = roleMgr;
        }

        public ViewResult Index()
        {

            return View(new UsersViewModel(User)
            {
                AppUsers = userManager.Users,
                Roles = roleManager.Roles,
            });
        }

        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(new EditUserViewModel(User)
                {
                    User = user,
                });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel vm)
        {
            string id = vm.User.Id;
            string email = vm.User.Email;
            string password = vm.Password;
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View(new EditUserViewModel(User)
            {
                User = user,
            });
        }

        private static CreateUserViewModel PreErrorData = null;
        public ViewResult CreateUser()
        {
            CreateUserViewModel vm = new CreateUserViewModel(User)
            {
                RoleNames = new List<string>(),
                SelectedRoles = new List<bool>(),
            };

            foreach (var role in roleManager.Roles)
            {
                vm.RoleNames.Add(role.Name);
                vm.SelectedRoles.Add(false);
            }

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

            return View(vm);
        }

        protected void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    AppUser userForRole = await userManager.FindByEmailAsync(user.Email);
                    if (userForRole != null)
                    {
                        for (int i = 0; i < model.SelectedRoles.Count; i++)
                        {
                            if (model.SelectedRoles[i])
                            {
                                result = await userManager.AddToRoleAsync(user, model.RoleNames[i]);
                                if (!result.Succeeded)
                                {
                                    AddErrorsFromResult(result);
                                }
                            }
                        }
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    PreErrorData = model;
                    var vm = new ErrorCreatingUserViewModel(User)
                    {
                        ErrorDescriptions = new List<string>(),
                    };
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        vm.ErrorDescriptions.Add(error.Description);
                    }

                    return View("ErrorCreatingUser", vm);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View("Index", userManager.Users);
        }

        ////////////////////////////////////////////////////////////////////////
        public IActionResult CreateRole() => View();

        [HttpPost]
        public async Task<IActionResult> CreateRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role); if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "No role found");
            }

            return View("Index", roleManager.Roles);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers; list.Add(user);
            }

            return View(new RoleEditViewModel(User)
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleModificationViewModel model)
        {
            IdentityResult result; if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }

                    }
                }

                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName); if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return await Edit(model.RoleId);
            }
        }
    }
}
