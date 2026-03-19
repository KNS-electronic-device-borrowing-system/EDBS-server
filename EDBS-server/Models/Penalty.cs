namespace EDBS_server.Models
{
    public class Penalty : BaseEntity
    {
        public int? BorrowingSlipId { get; set; }
        public int? UserId { get; set; }
        public decimal Amount { get; set; } = 0;
        public int DaysOverdue { get; set; } = 0;
        public string Status { get; set; } = "PENDING";
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        public BorrowingSlip? BorrowingSlip { get; set; }
        public User? User { get; set; }
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
    }
}
