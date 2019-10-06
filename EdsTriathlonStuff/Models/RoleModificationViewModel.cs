using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EdsTriathlonStuff.Models
{
    public class RoleModificationViewModel
    {
        public RoleModificationViewModel() { }
        public RoleModificationViewModel(ClaimsPrincipal identity) { }

        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
