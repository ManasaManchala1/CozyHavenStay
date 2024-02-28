using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cozy_Haven.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        public int HotelId { get; set; }

        public int UserId { get; set; }

        [Required]
        public float Rating { get; set; }

        public string? Comment { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Hotel? Hotel { get; set; }
        [JsonIgnore]
        public User? User { get; set; } 
    }
}
