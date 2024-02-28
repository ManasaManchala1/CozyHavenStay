using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Mappers
{
    public class AddBooking
    {
        private readonly Booking booking;

        public AddBooking(BookingDTO addBookingDTO)
        {
            booking = new Booking
            {
                UserId = addBookingDTO.UserId,
                RoomId = addBookingDTO.RoomId,
                CheckInDate = addBookingDTO.CheckInDate,
                CheckOutDate = addBookingDTO.CheckOutDate,
                Adults = addBookingDTO.Adults,
                Children = addBookingDTO.Children,
                TotalPrice = addBookingDTO.TotalPrice,
                Status = addBookingDTO.Status,
                BookedDate = DateTime.Now 
            };
        }

        public Booking GetBooking()
        {
            return booking;
        }
    }
}
