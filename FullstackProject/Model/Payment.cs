using System;
using System.Collections.Generic;

namespace FullstackProject.Model;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? PurchaseId { get; set; }

    public DateOnly? PaymentDate { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? PaymentMethod { get; set; }

    public virtual Purchase? Purchase { get; set; }
}
