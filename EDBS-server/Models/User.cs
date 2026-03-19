namespace EDBS_server.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public string? IdCardImageUrl { get; set; }
        public int? RoleId { get; set; }
        public bool IsLocked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        public Role? Role { get; set; }
        public User? CreatedByUser { get; set; }
        public User? UpdatedByUser { get; set; }
        public ICollection<User> CreatedUsers { get; set; } = new List<User>();
        public ICollection<User> UpdatedUsers { get; set; } = new List<User>();
        public ICollection<Category> CategoriesCreated { get; set; } = new List<Category>();
        public ICollection<Category> CategoriesUpdated { get; set; } = new List<Category>();
        public ICollection<Model> ModelsCreated { get; set; } = new List<Model>();
        public ICollection<Model> ModelsUpdated { get; set; } = new List<Model>();
        public ICollection<Product> ProductsCreated { get; set; } = new List<Product>();
        public ICollection<Product> ProductsUpdated { get; set; } = new List<Product>();
        public ICollection<Asset> AssetsCreated { get; set; } = new List<Asset>();
        public ICollection<Asset> AssetsUpdated { get; set; } = new List<Asset>();
        public ICollection<BorrowingSlip> BorrowingSlips { get; set; } = new List<BorrowingSlip>();
        public ICollection<BorrowingSlip> ProcessedBorrowingSlips { get; set; } = new List<BorrowingSlip>();
        public ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();
        public ICollection<Blacklist> Blacklists { get; set; } = new List<Blacklist>();
    }
}
