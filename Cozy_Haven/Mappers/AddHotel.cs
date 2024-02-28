using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Mappers
{
    public class AddHotel
    {
        private readonly Hotel newhotel;

        public AddHotel(HotelDTO hotel)
        {
            newhotel = new Hotel
            {
                DestinationId = hotel.DestinationId,
                OwnerId = hotel.OwnerId,
                Name = hotel.Name,
                Address = hotel.Address,
                Description = hotel.Description
            };
        }

        public Hotel GetHotel()
        {
            return newhotel;
        }
    }
}
