using System;
using System.Collections.Generic;

namespace FullstackProject.Model;

public partial class Developer
{
    public int DeveloperId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
