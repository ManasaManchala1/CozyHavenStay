namespace Cozy_Haven.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string ImagePath { get; set; }

        // Optional foreign keys to associate with Hotel and Room
        public int? HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }

        public int? RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}
