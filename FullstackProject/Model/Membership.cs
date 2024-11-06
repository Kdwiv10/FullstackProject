using System;
using System.Collections.Generic;

namespace FullstackProject.Model
{
    public partial class Membership
    {
        public int MembershipId { get; set; }
        public int? UserId { get; set; }
        public string? Type { get; set; }
        public int? Price { get; set; }
        public string? Description { get; set; }

        // Navigation property for User
        public virtual User? User { get; set; }

        // Computed property for discount based on membership type
        public decimal Discount
        {
            get
            {
                return Type switch
                {
                    "Gold" => 0.14m,   
                    "Silver" => 0.09m, 
                    "Bronze" => 0.06m, 
                    _ => 0m            
                };
            }
        }
    }
}
