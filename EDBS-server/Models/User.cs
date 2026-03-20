using System;
using System.Collections.Generic;

namespace EDBS_server.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? AvatarUrl { get; set; }

    public string? IdCardImageUrl { get; set; }

    public int? RoleId { get; set; }

    public bool? IsLocked { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public bool? IsVerified { get; set; } = false;
    public string? VerificationToken { get; set; }
    public DateTime? VerificationTokenExpiresAt { get; set; }

    public virtual ICollection<Asset> AssetCreatedByNavigations { get; set; } = new List<Asset>();

    public virtual ICollection<Asset> AssetUpdatedByNavigations { get; set; } = new List<Asset>();

    public virtual ICollection<Blacklist> BlacklistCreatedByNavigations { get; set; } = new List<Blacklist>();

    public virtual ICollection<Blacklist> BlacklistUpdatedByNavigations { get; set; } = new List<Blacklist>();

    public virtual ICollection<Blacklist> BlacklistUsers { get; set; } = new List<Blacklist>();

    public virtual ICollection<BorrowingSlip> BorrowingSlipCreatedByNavigations { get; set; } = new List<BorrowingSlip>();

    public virtual ICollection<BorrowingSlip> BorrowingSlipProcessedByNavigations { get; set; } = new List<BorrowingSlip>();

    public virtual ICollection<BorrowingSlip> BorrowingSlipUpdatedByNavigations { get; set; } = new List<BorrowingSlip>();

    public virtual ICollection<BorrowingSlip> BorrowingSlipUsers { get; set; } = new List<BorrowingSlip>();

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryUpdatedByNavigations { get; set; } = new List<Category>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<Model> ModelCreatedByNavigations { get; set; } = new List<Model>();

    public virtual ICollection<Model> ModelUpdatedByNavigations { get; set; } = new List<Model>();

    public virtual ICollection<Penalty> PenaltyCreatedByNavigations { get; set; } = new List<Penalty>();

    public virtual ICollection<Penalty> PenaltyUpdatedByNavigations { get; set; } = new List<Penalty>();

    public virtual ICollection<Penalty> PenaltyUsers { get; set; } = new List<Penalty>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductUpdatedByNavigations { get; set; } = new List<Product>();

    public virtual Role? Role { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
