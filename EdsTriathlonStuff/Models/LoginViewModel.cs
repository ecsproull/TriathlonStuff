using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;

namespace EdsTriathlonStuff.Models
{
    public class LoginViewModel
    {
        public LoginViewModel(){ }
        public LoginViewModel(ClaimsPrincipal identity) { }
        [Required]
        [UIHint("email")]
        public string Email { get; set; }
        [Required]
        [UIHint("password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
