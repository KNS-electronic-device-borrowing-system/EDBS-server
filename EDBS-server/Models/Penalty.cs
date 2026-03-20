using System;
using System.Collections.Generic;

namespace EDBS_server.Models;

public partial class Penalty
{
    public int Id { get; set; }

    public int? BorrowingSlipId { get; set; }

    public int? UserId { get; set; }

    public decimal? Amount { get; set; }

    public int? DaysOverdue { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual BorrowingSlip? BorrowingSlip { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual User? User { get; set; }
}
