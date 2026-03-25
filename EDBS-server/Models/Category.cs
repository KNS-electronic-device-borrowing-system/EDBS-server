using System.ComponentModel.DataAnnotations;

namespace EDBS_server.Models
{
    public class Category : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}
