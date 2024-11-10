using System;
using System.Collections.Generic;

namespace FullstackProject.Model
{
    public partial class Membership
    {
        public int MembershipId { get; set; }
        public int? UserId { get; set; }
        public string? Type { get; set; }
        public float? DiscountRate { get; set; }
        public int? Price { get; set; }
        public string? Description { get; set; }

        // Navigation property for User
        public virtual User? User { get; set; }

        
            }
        }
  