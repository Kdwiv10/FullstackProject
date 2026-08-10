using FullStack_Shangri.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FullStack_Shangri.ViewModels
{
    public class UserMembershipViewModel
    {
        [Key]
        public int UserMembershipId { get; set; }
        public string UserId { get; set; }
        public int MembershipId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime MembershipExpiryDate { get; set; }

        public virtual IdentityUser User { get; set; }
        public virtual Memberships Membership { get; set; }
    }

}
