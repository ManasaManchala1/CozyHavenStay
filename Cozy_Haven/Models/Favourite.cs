using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cozy_Haven.Models
{
    public class Favourite
    {
        [Key]
        public int FavouriteId { get; set; }

        public int UserId { get; set; }

        public int HotelId { get; set; }

        // Navigation properties
        public User? User { get; set; } 
        public Hotel? Hotel { get; set; } 
    }
}
