using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Interfaces
{
    public interface IBookingService
    {
        public Task<Booking> GetBooking(int id);
        public Task<List<Booking>> GetAllBookings();
        public Task<Booking> AddBooking(BookingDTO booking,string username);
        public Task<Booking> UpdateBookingStatus(int id,string status);
        public Task<Booking> DeleteBooking(int id);
        //public Task<List<Booking>> GetBookingsByUserId(int userid);
        public Task<List<Booking>> GetBookingsByRoomId(int roomid);
        public Task<int> GetBookingsCount();
        //public Task<int> AvailableRoomsCount();
        public Task<List<Booking>> GetHotelBookings(int hotelId);
        public Task<bool> IsRoomAvailable(int roomId, DateTime checkInDate, DateTime checkOutDate);
        public Task<int> GetTotalBookings();
        public Task<float> GetTotalRevenue();
        public Task<IEnumerable<object>> GetLineChartData();
        public Task<bool> CancelBooking(int bookingId);
        public Task<List<Booking>> GetCancelledHotelBookings(int hotelId);
        public Task<Booking> UpdateBooking(int id, Booking updatedBooking);
    }
}
