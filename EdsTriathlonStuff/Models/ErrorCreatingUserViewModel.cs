using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;

namespace EdsTriathlonStuff.Models
{
    public class ErrorCreatingUserViewModel
    {
        public ErrorCreatingUserViewModel(ClaimsPrincipal identity) { }

        public List<string> ErrorDescriptions { get; set; }
    }
}
