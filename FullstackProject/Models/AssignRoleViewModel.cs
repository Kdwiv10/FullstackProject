using Microsoft.AspNetCore.Identity;

namespace FullStack_Shangri.Models
{
    public class AssignRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public List<string> SelectedRoleIds { get; set; }
    }

}
