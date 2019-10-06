using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EdsTriathlonStuff.Models
{
    public class CreateUserViewModel
    {
        public CreateUserViewModel() { }
        public CreateUserViewModel(ClaimsPrincipal identity) { }
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter valid email address.")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address")]
        [UIHint("email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }
        [Display(Name = "Confirm ")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public List<string> RoleNames { get; set; }
        public List<bool> SelectedRoles { get; set; }
        public bool AllowRoleSelection { get; set; } = true;
    }
}
