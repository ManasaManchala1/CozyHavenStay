using Cozy_Haven.Exceptions;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System;
using Cozy_Haven.Models.DTOs;
using Cozy_Haven.Mappers;

namespace Cozy_Haven.Services
{
    public class HotelService : IHotelService
    {
        private readonly IRepository<int, Hotel> _repository;
        private readonly ILogger<HotelService> _logger;

        public HotelService(IRepository<int, Hotel> repository, ILogger<HotelService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<Hotel> AddHotel(HotelDTO hotel)
        {
            Hotel newhotel=new AddHotel(hotel).GetHotel();
            newhotel=await _repository.Add(newhotel);
            return newhotel;
        }

        public async Task<Hotel> DeleteHotel(int id)
        {
            var hotel = await GetHotel(id);
            if (hotel != null)
            {
                return await _repository.Delete(id);
            }
            throw new NoHotelFoundException();

        }

        public async Task<List<Hotel>> GetAllHotels()
        {
            var hotels = await _repository.GetAll();
            if (hotels != null) { return hotels; }
            throw new NoHotelFoundException();
        }

        public Task<Hotel> GetHotel(int id)
        {
            var hotel = _repository.GetById(id);
            if (hotel != null) { return hotel; }
            throw new NoHotelFoundException();
        }

        //public async Task<List<Hotel>> GetHotelsByDestinationId(int destinationId)
        //{
        //    var hotels=await GetAllHotels();
        //    if (hotels == null) { throw new NoHotelFoundException(); }
        //    List<Hotel> hotels1 = new List<Hotel>();
        //    foreach(var hotel in hotels)
        //    {
        //        if (hotel.DestinationId == destinationId) { hotels1.Add(hotel); }
        //    }
        //    return hotels1;
        //}

        public async Task<Hotel> UpdateHotelDescription(int id, string description)
        {
            var hotel = await GetHotel(id);
            if (hotel != null)
            {
                hotel.Description = description;
                return await _repository.Update(hotel);
            }
            throw new NoHotelFoundException();
        }
        public async Task<Hotel> UpdateHotelOwner(int id, int ownerId)
        {
            _logger.LogInformation("Updating owner for hotel with ID: {HotelId}", id);
            var hotel = await GetHotel(id);
            if (hotel != null)
            {
                hotel.OwnerId = ownerId;
                await _repository.Update(hotel);
                return hotel;
            }
            throw new NoHotelFoundException(id);
        }
        public async Task<ICollection<Review>> GetHotelReviews(int hotelId)
        {
            _logger.LogInformation("Getting reviews for hotel with ID: {HotelId}", hotelId);
            var hotel = await GetHotel(hotelId);
            if (hotel != null)
            {
                return hotel.Reviews;
            }
            throw new NoHotelFoundException(hotelId);
        }
        public async Task<ICollection<HotelAmenity>> GetHotelAmenities(int hotelId)
        {
            _logger.LogInformation("Getting amenities for hotel with ID: {HotelId}", hotelId);
            var hotel = await GetHotel(hotelId);
            if (hotel != null)
            {
                return hotel.Amenities;
            }
            throw new NoHotelFoundException(hotelId);
        }
        public async Task<ICollection<Hotel>> GetRecommendedHotels()
        {
            var allHotels = await GetAllHotels();
            // Example logic: recommend hotels with average rating above a certain threshold
            var recommendedHotels = allHotels
                .Where(h => h.Reviews.Any()) // Assuming hotels with at least one review are recommended
                .OrderByDescending(h => h.Reviews.Average(r => r.Rating)) // Ordering by average rating
                .Take(5) // Limiting to top 5 recommended hotels
                .ToList();

            return recommendedHotels;
        }
        public async Task<ICollection<Room>> GetRoomsByHotelId(int hotelId)
        {
            _logger.LogInformation("Getting rooms for hotel with ID: {HotelId}", hotelId);
            var hotel = await GetHotel(hotelId);
            if (hotel != null)
            {
                return hotel.Rooms;
            }
            throw new NoHotelFoundException(hotelId);
        }
        public async Task<Hotel> UpdateHotelDetails(Hotel hotel)
        {
            _logger.LogInformation("Updating details for hotel with ID: {HotelId}", hotel.HotelId);
            var existingHotel = await GetHotel(hotel.HotelId);
            if (existingHotel != null)
            {
                existingHotel.Name = hotel.Name;
                existingHotel.Address = hotel.Address;
                existingHotel.Description = hotel.Description;

                await _repository.Update(existingHotel);
                _logger.LogInformation("Hotel details updated successfully: {HotelId}", hotel.HotelId);
                return existingHotel;
            }
            throw new NoHotelFoundException();
        }
        public async Task<ICollection<Booking>> GetHotelBookings(int hotelId)
        {
            _logger.LogInformation("Getting bookings for hotel with ID: {HotelId}", hotelId);
            var hotel = await GetHotel(hotelId);
            if (hotel != null)
            {
                var bookings = hotel.Rooms.SelectMany(r => r.Bookings).ToList();
                return bookings;
            }
            throw new NoHotelFoundException(hotelId);
        }
        //The SelectMany method is used to flatten a sequence of sequences into a single sequence.In the context of retrieving bookings for a hotel, if each room in the hotel has a collection of bookings, using SelectMany allows us to concatenate all the booking collections into a single list of bookings for the hotel.This is useful when you want to aggregate or combine multiple collections into one.

        //Here's how it works in the context of the GetHotelBookings method:

        //We start with the hotel.Rooms collection, which is a collection of rooms in the hotel.
        //For each room, we access its Bookings property, which is a collection of bookings for that room.
        //By using SelectMany, we flatten these collections of bookings into a single list of all bookings for all rooms in the hotel.
        //    
        public async Task<ICollection<Hotel>> GetHotelsByOwnerId(int ownerId)
        {
            var hotels = await GetAllHotels();
            hotels = hotels.Where(h => h.OwnerId == ownerId).ToList();
            return hotels;
        }

    }
}
