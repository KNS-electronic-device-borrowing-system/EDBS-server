using System;
using System.Collections.Generic;

namespace EDBS_server.Models;

public partial class BorrowingSlip
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? AssetId { get; set; }

    public DateTime? BorrowDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ActualReturnDate { get; set; }

    public string? Reason { get; set; }

    public string? Status { get; set; }

    public int? ProcessedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Asset? Asset { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();

    public virtual User? ProcessedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual User? User { get; set; }
}
