using System;
using System.Collections.Generic;

namespace FullStack_Shangri.Models;

public partial class Game
{
    public int GameId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public decimal? Price { get; set; }

    public int? DeveloperId { get; set; }

    public int? Stock { get; set; }

    public string? ImageFileName { get; set; }

    public int? GenreId { get; set; }

    public virtual Developer? Developer { get; set; }

    public virtual ICollection<GameLibrary> GameLibraries { get; set; } = new List<GameLibrary>();

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
