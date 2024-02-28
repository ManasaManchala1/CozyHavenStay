using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cozy_Haven.Models
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }

        [Required]
        public int DestinationId { get; set; }

        public int OwnerId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }
        
        public string Description { get; set; }


        // Navigation properties
        [JsonIgnore]
        public Destination? Destination { get; set; }
        [JsonIgnore]
        public User? Owner { get; set; }
        [JsonIgnore]
        public ICollection<Room>? Rooms { get; set; }
        [JsonIgnore]
        public ICollection<Review>? Reviews { get; set; }
        [JsonIgnore]
        public ICollection<Favourite>? Favorites { get; set; }
        [JsonIgnore]
        public ICollection<HotelAmenity>? Amenities { get; set; }
        [JsonIgnore]
        public ICollection<HotelImage> Images { get; set; }
    }
}
