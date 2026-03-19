namespace EDBS_server.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
