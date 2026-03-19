namespace EDBS_server.Models
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public int? ModelId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public Model? Model { get; set; }
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
        public ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}
