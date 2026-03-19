namespace EDBS_server.Models
{
    public class Asset : BaseEntity
    {
        public int? ProductId { get; set; }
        public string AssetTag { get; set; } = null!;
        public string? ManufacturerSerial { get; set; }
        public string Status { get; set; } = "READY";
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
        public ICollection<BorrowingSlip> BorrowingSlips { get; set; } = new List<BorrowingSlip>();
    }
}
