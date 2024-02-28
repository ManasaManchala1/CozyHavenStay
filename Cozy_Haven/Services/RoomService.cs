using Cozy_Haven.Exceptions;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Mappers;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<int, Room> _repository;
        private readonly ILogger<IRoomService> _logger;

        public RoomService(IRepository<int, Room> repository,ILogger<IRoomService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<Room> AddRoom(RoomDTO room)
        {
            Room newroom = new AddRoom(room).GetRoom();
            newroom=await _repository.Add(newroom);
            return newroom;
        }

        public async Task<Room> DeleteRoom(int id)
        {
            var room = await GetRoom(id);
            if (room != null)
            {
                await _repository.Delete(id);
                return room;
            }
            throw new NoRoomFoundException();
        }

        public async Task<List<Room>> GetAllRooms()
        {
            var rooms = await _repository.GetAll();
            if (rooms != null) { return rooms; }
            throw new NoRoomFoundException();
        }

        public async Task<Room> GetRoom(int id)
        {
            var room = await _repository.GetById(id);
            if (room != null) { return room; }
            throw new NoRoomFoundException();
        }

        public async Task<ICollection<Booking>> GetRoomBookings(int id)
        {
            var room = await GetRoom(id);
            if (room == null) { throw new NoRoomFoundException(); }
            var bookings = room.Bookings;
            if (bookings == null) { throw new NoBookingFoundException(id); }
            return bookings;
        }

        public async Task<Room> UpdateRoomPrice(int id, int price)
        {
            var room = await GetRoom(id);
            if (room != null)
            {
                room.BaseFare = price;
                await _repository.Update(room);
                return room;
            }
            throw new NoRoomFoundException();
        }
        public async Task<ICollection<RoomAmenity>> GetRoomAmenities(int id)
        {
            var room = await GetRoom(id);
            if (room == null) throw new NoRoomFoundException();
            var amenities = room.Amenities;
            if (amenities == null) { throw new NoAmenityFoundException(); }
            return amenities;
        }
        public async Task<Room> UpdateRoomDetails(Room room)
        {
            var existingRoom = await GetRoom(room.RoomId);
            if (existingRoom == null) throw new NoRoomFoundException();
            existingRoom.RoomSize = room.RoomSize;
            existingRoom.RoomType = room.RoomType;
            existingRoom.BedType = room.BedType;
            existingRoom.BaseFare = room.BaseFare;
            existingRoom.MaxOccupancy = room.MaxOccupancy;
            existingRoom.AC = room.AC;
            existingRoom.Available = room.Available;
            await _repository.Update(existingRoom);
            return existingRoom;
        }
        public async Task<int> GetAvailableRoomsCount()
        {
            var rooms = await _repository.GetAll();
            if (rooms == null) throw new NoRoomFoundException();
            var availableRoomsCount = rooms.Count(r => r.Available);
            return availableRoomsCount;
        }
        public async Task<IEnumerable<object>> GetDonutChartData()
        {
            var rooms = await _repository.GetAll();
            var data = rooms
                .GroupBy(r => r.RoomType)
                .Select(g => new { label = g.Key, value = g.Count() })
                .ToList();

            return data;
        }



    }
}
