using System.ComponentModel.DataAnnotations;

namespace EDBS_server.Models
{
    public class Transaction : BaseEntity
    {
        public int BorrowerId { get; set; }
        public virtual User Borrower { get; set; } = null!;

        public int? ApprovedById { get; set; }
        public virtual User? Approver { get; set; }

        [Required]
        public string Reason { get; set; } = null!;

        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpireDate { get; set; }
        public DateTime DueDate { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Active, Completed, Canceled

        public string? AdminNotes { get; set; }

        public virtual ICollection<TransactionDetail> Details { get; set; } = new List<TransactionDetail>();
    }
}
