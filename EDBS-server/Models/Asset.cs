using System.ComponentModel.DataAnnotations;

namespace EDBS_server.Models
{
    public class Asset : BaseEntity
    {
        [Required, MaxLength(50)]
        public string AssetCode { get; set; } = null!;

        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Available"; // Available, Reserved, In-use, Maintenance, Broken

        [MaxLength(255)]
        public string? QrCode { get; set; }
        public string? Description { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
    }
}
