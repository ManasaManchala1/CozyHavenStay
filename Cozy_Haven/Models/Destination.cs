using System.ComponentModel.DataAnnotations;

namespace Cozy_Haven.Models
{
    public class Destination
    {
        [Key]
        public int DestinationId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        // Navigation property
        public ICollection<Hotel>? Hotels { get; set; } 
    }
}
