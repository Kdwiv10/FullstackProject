using System;
using System.Collections.Generic;

namespace FullstackProject.Model;

public partial class Membership
{
    public int MembershipId { get; set; }

    public int? UserId { get; set; }

    public string? Type { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual User? User { get; set; }
}
