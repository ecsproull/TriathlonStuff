using EdsTriathlonStuff.App_Code;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace EdsTriathlonStuff.Models
{
    public class RoleEditViewModel
    {
        public RoleEditViewModel() { }
        public RoleEditViewModel(ClaimsPrincipal identity) { }

        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
}