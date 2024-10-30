using System;
using System.Collections.Generic;

namespace FullstackProject.Model;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public int? GameId { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? User { get; set; }
}
