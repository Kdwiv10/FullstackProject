using System;
using System.Collections.Generic;

namespace FullStack_Shangri.Models;

public partial class Memberships
{
    public int MembershipId { get; set; }

    public string? Type { get; set; }

    public int? Price { get; set; }

    public string? Description { get; set; }

    public double? DiscountRate { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();
}
