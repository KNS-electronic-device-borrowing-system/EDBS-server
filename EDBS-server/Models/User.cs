using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDBS_server.Models
{
    public class User : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(255)]
        public string Password { get; set; } = null!;

        [Required, MaxLength(50)]
        public string EmployeeCode { get; set; } = null!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;

        [MaxLength(500)]
        public string? AvatarUrl { get; set; }

        [MaxLength(500)]
        public string? IdCardImageUrl { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public bool? IsVerified { get; set; } = false;

        [MaxLength(255)]
        public string? VerificationToken { get; set; }

        public DateTime? VerificationTokenExpiresAt { get; set; }

        // Navigation properties cho mượn và duyệt
        public virtual ICollection<Transaction> BorrowedTransactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Transaction> ApprovedTransactions { get; set; } = new List<Transaction>();
    }
}