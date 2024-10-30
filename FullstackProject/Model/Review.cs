using System;
using System.Collections.Generic;

namespace FullstackProject.Model;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? GameId { get; set; }

    public int? UserId { get; set; }

    public int? Rating { get; set; }

    public string? Comments { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? User { get; set; }
}
