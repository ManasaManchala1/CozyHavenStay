namespace Cozy_Haven.Models.DTOs
{
    public class RoomDTO
    {
        //public int RoomId { get; set; }
        public int HotelId { get; set; }
        public float RoomSize { get; set; }
        public string RoomType { get; set; }
        public string BedType { get; set; }
        public float BaseFare { get; set; }
        public int MaxOccupancy { get; set; }
        public bool AC { get; set; }
        public bool Available { get; set; }
    }
}
