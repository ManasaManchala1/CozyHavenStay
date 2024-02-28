using System.ComponentModel.DataAnnotations;

namespace Cozy_Haven.Models
{
    public class RoomImage
    {
        [Key]
        public int ImageId { get; set; }
        public byte[] ImagePath { get; set; }
        public int? RoomId { get; set; }
        public virtual Room Room { get; set; }

    }
}