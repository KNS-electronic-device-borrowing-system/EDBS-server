namespace EDBS_server.Models
{
    public class Blacklist : BaseEntity
    {
        public int? UserId { get; set; }
        public string? Reason { get; set; }
        public DateTime BannedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
    }
}
