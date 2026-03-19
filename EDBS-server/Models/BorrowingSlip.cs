namespace EDBS_server.Models
{
    public class BorrowingSlip : BaseEntity
    {
        public int? UserId { get; set; }
        public int? AssetId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "PENDING";
        public int? ProcessedBy { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Asset? Asset { get; set; }
        public User? ProcessedByUser { get; set; }
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
        public ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();
    }
}
