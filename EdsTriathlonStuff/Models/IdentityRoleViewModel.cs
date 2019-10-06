using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace EdsTriathlonStuff.Models
{
    public class IdentityRoleViewModel 
    {
        public IdentityRoleViewModel()  { }
        public IdentityRoleViewModel(ClaimsPrincipal identity)  { }

        public IEnumerable<IdentityRole> Roles {get;set;}
    }
}
