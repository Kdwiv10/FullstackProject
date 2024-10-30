using System;
using System.Collections.Generic;

namespace FullstackProject.Model;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int? UserId { get; set; }

    public int? GameId { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public decimal? PricePaid { get; set; }

    public virtual Game? Game { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
