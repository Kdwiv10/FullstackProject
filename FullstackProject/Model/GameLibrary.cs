using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FullstackProject.Model;

public partial class GameLibrary
{
    [Key]
    public int LibraryId { get; set; }

    public int? UserId { get; set; }

    public int? GameId { get; set; }

    public DateOnly? DateAdded { get; set; }

    public decimal? PricePaid { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? User { get; set; }
}
