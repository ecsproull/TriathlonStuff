using EdsTriathlonStuff.App_Code;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace EdsTriathlonStuff.Models
{
    public class UsersViewModel
    {
        public UsersViewModel() { }
        public UsersViewModel(ClaimsPrincipal identity) { }

        public IEnumerable<AppUser> AppUsers { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
