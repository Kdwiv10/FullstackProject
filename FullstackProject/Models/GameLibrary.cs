using System;
using System.Collections.Generic;

namespace FullStack_Shangri.Models;

public partial class GameLibrary
{
    public int LibraryId { get; set; }

    public int? UserId { get; set; }

    public int? GameId { get; set; }

    public DateOnly? DateAdded { get; set; }

    public decimal? PricePaid { get; set; }

    public virtual Game? Game { get; set; }
}
