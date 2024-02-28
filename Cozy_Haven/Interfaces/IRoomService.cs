using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Interfaces
{
    public interface IRoomService
    {
        public Task<Room> GetRoom(int id);
        public Task<List<Room>> GetAllRooms();
        public Task<Room> AddRoom(RoomDTO room);
        public Task<Room> UpdateRoomPrice(int id, int price);
        public Task<Room> DeleteRoom(int id);
        public Task<ICollection<Booking>> GetRoomBookings(int id);
        public Task<ICollection<RoomAmenity>> GetRoomAmenities(int id);
        public Task<Room> UpdateRoomDetails(Room room);
        public Task<int> GetAvailableRoomsCount();
        public Task<IEnumerable<object>> GetDonutChartData();
    }
}
