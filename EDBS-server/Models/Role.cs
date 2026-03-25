using System.ComponentModel.DataAnnotations;

namespace EDBS_server.Models
{
    public class Role : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
