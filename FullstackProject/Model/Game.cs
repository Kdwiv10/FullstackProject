using System;
using System.Collections.Generic;

namespace FullstackProject.Model;

public partial class Game
{
    public int GameId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? ReleaseDate { get; set; } 

    public decimal? Price { get; set; }

    public string? Genre { get; set; }

    public int? DeveloperId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Developer? Developer { get; set; }

    public virtual ICollection<GameLibrary> GameLibraries { get; set; } = new List<GameLibrary>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
