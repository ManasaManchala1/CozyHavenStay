using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Mappers
{
    public class AddRoom
    {
        private readonly Room room;

        public AddRoom(RoomDTO addRoomDTO)
        {
            room = new Room
            {
                HotelId = addRoomDTO.HotelId,
                RoomSize = addRoomDTO.RoomSize,
                RoomType = addRoomDTO.RoomType,
                BedType = addRoomDTO.BedType,
                BaseFare = addRoomDTO.BaseFare,
                MaxOccupancy = addRoomDTO.MaxOccupancy,
                AC = addRoomDTO.AC,
                Available = true
            };
        }

        public Room GetRoom()
        {
            return room;
        }
    }
}
