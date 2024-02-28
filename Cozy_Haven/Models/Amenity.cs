using System.ComponentModel.DataAnnotations;

namespace Cozy_Haven.Models
{
    public class Amenity
    {
        [Key]
        public int AmenityId { get; set; }

        [Required]
        public string Name { get; set; }
        //navigation properties
        public ICollection<RoomAmenity>? Rooms { get; set; } 
        public ICollection<HotelAmenity>? Hotels { get; set; } 

    }
}
