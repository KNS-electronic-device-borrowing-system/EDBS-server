using System;
using System.Collections.Generic;

namespace EDBS_server.Models;

public partial class Blacklist
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Reason { get; set; }

    public DateTime? BannedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual User? User { get; set; }
}
