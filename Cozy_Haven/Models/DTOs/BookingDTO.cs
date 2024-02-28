namespace Cozy_Haven.Models.DTOs
{
    public class BookingDTO
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public float TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
