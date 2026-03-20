using System;
using System.Collections.Generic;

namespace EDBS_server.Models;

public partial class Asset
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string AssetTag { get; set; } = null!;

    public string? ManufacturerSerial { get; set; }

    public string? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<BorrowingSlip> BorrowingSlips { get; set; } = new List<BorrowingSlip>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
