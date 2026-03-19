namespace EDBS_server.Models
{
    public class Model : BaseEntity
    {
        public string ModelName { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
