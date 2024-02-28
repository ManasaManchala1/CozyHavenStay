using Cozy_Haven.Exceptions;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;
using Cozy_Haven.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cozy_Haven_Testing
{
    public class Hotel_Testing
    {
        private IHotelService _hotelService;
        private Mock<IRepository<int, Hotel>> _mockRepo;
        private Mock<ILogger<HotelService>> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<int, Hotel>>();
            _mockLogger = new Mock<ILogger<HotelService>>();
            _hotelService = new HotelService(_mockRepo.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddHotelSuccessTest()
        {
            // Arrange
            var hotelDto = new HotelDTO { Name = "New Hotel", Address = "123 Main St", Description = "A cozy hotel" };
            var hotel = new Hotel { HotelId = 1, Name = "New Hotel", Address = "123 Main St", Description = "A cozy hotel" };

            _mockRepo.Setup(x => x.Add(It.IsAny<Hotel>())).ReturnsAsync(hotel);

            // Act
            var result = await _hotelService.AddHotel(hotelDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.HotelId);
            Assert.AreEqual("New Hotel", result.Name);
            Assert.AreEqual("123 Main St", result.Address);
            Assert.AreEqual("A cozy hotel", result.Description);
        }

        [Test]
        public async Task DeleteHotelSuccessTest()
        {
            // Arrange
            var hotelId = 1;
            var hotel = new Hotel { HotelId = hotelId, Name = "Hotel to Delete" };
            _mockRepo.Setup(x => x.GetById(hotelId)).ReturnsAsync(hotel);
            _mockRepo.Setup(x => x.Delete(hotelId)).ReturnsAsync(hotel);

            // Act
            var result = await _hotelService.DeleteHotel(hotelId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(hotelId, result.HotelId);
            Assert.AreEqual("Hotel to Delete", result.Name);
        }

        [Test]
        public async Task GetAllHotelsSuccessTest()
        {
            // Arrange
            var hotels = new List<Hotel> { new Hotel { HotelId = 1 }, new Hotel { HotelId = 2 } };
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(hotels);

            // Act
            var result = await _hotelService.GetAllHotels();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetHotelSuccessTest()
        {
            // Arrange
            var hotelId = 1;
            var hotel = new Hotel { HotelId = hotelId };
            _mockRepo.Setup(x => x.GetById(hotelId)).ReturnsAsync(hotel);

            // Act
            var result = await _hotelService.GetHotel(hotelId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(hotelId, result.HotelId);
        }


        [Test]
        public async Task UpdateHotelDescriptionSuccessTest()
        {
            // Arrange
            var hotelId = 1;
            var description = "Updated Description";
            var hotel = new Hotel { HotelId = hotelId, Description = "Initial Description" };
            _mockRepo.Setup(x => x.GetById(hotelId)).ReturnsAsync(hotel);
            _mockRepo.Setup(x => x.Update(It.IsAny<Hotel>())).ReturnsAsync((Hotel h) => h);

            // Act
            var result = await _hotelService.UpdateHotelDescription(hotelId, description);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(hotelId, result.HotelId);
            Assert.AreEqual(description, result.Description);
        }
    }
}
