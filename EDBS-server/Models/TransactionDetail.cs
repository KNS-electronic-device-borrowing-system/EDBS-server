using System.ComponentModel.DataAnnotations;

namespace EDBS_server.Models
{
    public class TransactionDetail : BaseEntity
    {
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; } = null!;

        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; } = null!;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Borrowed, Returned

        public DateTime? CheckoutDate { get; set; }
        public DateTime? CheckinDate { get; set; }

        [MaxLength(50)]
        public string? ReturnCondition { get; set; } // Normal, Broken, Lost
    }
}
