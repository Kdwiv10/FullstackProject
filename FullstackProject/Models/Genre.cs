using System;
using System.Collections.Generic;

namespace FullStack_Shangri.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
