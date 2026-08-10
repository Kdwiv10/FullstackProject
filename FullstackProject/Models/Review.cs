using System;
using System.Collections.Generic;

namespace FullStack_Shangri.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? GameId { get; set; }

    public int? UserId { get; set; }

    public int? Rating { get; set; }

    public string? Comments { get; set; }

    public virtual Game? Game { get; set; }
}
