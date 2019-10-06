using EdsTriathlonStuff.App_Code;
using System.Security.Claims;

namespace EdsTriathlonStuff.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel() { }
        public EditUserViewModel(ClaimsPrincipal identity) { }

        public AppUser User { get; set; }
        public string Password { get; set; }
    }
}
