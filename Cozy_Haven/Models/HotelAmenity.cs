﻿namespace Cozy_Haven.Models
{
    public class HotelAmenity
    {
        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }

        public int AmenityId { get; set; }
        public Amenity? Amenity { get; set; }
    }
}
