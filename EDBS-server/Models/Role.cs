namespace EDBS_server.Models
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = null!;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
