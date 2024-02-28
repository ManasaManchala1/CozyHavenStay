using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Text.Json.Serialization;

namespace Cozy_Haven.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        [Required]
        public float RoomSize {  get; set; }

        [Required]
        public string RoomType { get; set; }
        [Required]
        public string BedType { get; set; }

        [Required]
        public float BaseFare { get; set; }

        [Required]
        public int MaxOccupancy { get; set; }

        public bool AC { get; set; }
        public bool Available {  get; set; }

        // Navigation property
        [JsonIgnore]
        public Hotel? Hotel { get; set; }
        [JsonIgnore]
        public ICollection<Booking>? Bookings { get; set; }
        [JsonIgnore]
        public ICollection<RoomAmenity>? Amenities { get; set; }
        [JsonIgnore]
        public ICollection<RoomImage> Images {  get; set; }
    }
}
