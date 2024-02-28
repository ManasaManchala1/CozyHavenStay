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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cozy_Haven_Testing
{
    public class Room_Testing
    {
        private IRoomService _roomService;
        private Mock<IRepository<int, Room>> _mockRepo;
        private Mock<ILogger<IRoomService>> _logger;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<int, Room>>();
            _logger=new Mock<ILogger<IRoomService>>();
            _roomService = new RoomService(_mockRepo.Object,_logger.Object);
        }

        [Test]
        public async Task AddRoomSuccessTest()
        {
            // Arrange
            var roomDto = new RoomDTO { HotelId=1,RoomType = "Single", RoomSize=90,BaseFare = 100, MaxOccupancy=2 };
            var room = new Room { RoomId = 1, HotelId=1, RoomType = "Single", RoomSize=90, BaseFare = 100, MaxOccupancy=2};

            _mockRepo.Setup(x => x.Add(It.IsAny<Room>())).ReturnsAsync(room);

            // Act
            var result = await _roomService.AddRoom(roomDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.RoomId);
            Assert.AreEqual("Single", result.RoomType);
            Assert.AreEqual(100, result.BaseFare);
        }

        [Test]
        public async Task DeleteRoomSuccessTest()
        {
            // Arrange
            var roomId = 1;
            var room = new Room { RoomId = roomId, RoomType = "Single" };
            _mockRepo.Setup(x => x.GetById(roomId)).ReturnsAsync(room);
            _mockRepo.Setup(x => x.Delete(roomId)).ReturnsAsync(room);

            // Act
            var result = await _roomService.DeleteRoom(roomId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(roomId, result.RoomId);
            Assert.AreEqual("Single", result.RoomType);
        }

        [Test]
        public async Task GetAllRoomsSuccessTest()
        {
            // Arrange
            var rooms = new List<Room> { new Room { RoomId = 1 }, new Room { RoomId = 2 } };
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(rooms);

            // Act
            var result = await _roomService.GetAllRooms();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetRoomSuccessTest()
        {
            // Arrange
            var roomId = 1;
            var room = new Room { RoomId = roomId };
            _mockRepo.Setup(x => x.GetById(roomId)).ReturnsAsync(room);

            // Act
            var result = await _roomService.GetRoom(roomId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(roomId, result.RoomId);
        }

        [Test]
        public void GetRoomNotFoundExceptionTest()
        {
            // Arrange
            var roomId = 1;
            _mockRepo.Setup(x => x.GetById(roomId)).ReturnsAsync((Room)null);

            // Act & Assert
            Assert.ThrowsAsync<NoRoomFoundException>(async () => await _roomService.GetRoom(roomId));
        }

        [Test]
        public async Task UpdateRoomPriceSuccessTest()
        {
            // Arrange
            var roomId = 1;
            var price = 150;
            var room = new Room { RoomId = roomId, BaseFare = 100 };
            _mockRepo.Setup(x => x.GetById(roomId)).ReturnsAsync(room);
            _mockRepo.Setup(x => x.Update(It.IsAny<Room>())).ReturnsAsync((Room r) => r);

            // Act
            var result = await _roomService.UpdateRoomPrice(roomId, price);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(roomId, result.RoomId);
            Assert.AreEqual(price, result.BaseFare);
        }

        //[Test]
        //public async Task GetAvailableRoomsSuccessTest()
        //{
        //    // Arrange
        //    var rooms = new List<Room>
        //    {
        //        new Room { RoomId = 1, Available = true },
        //        new Room { RoomId = 2, Available = false },
        //        new Room { RoomId = 3, Available = true }
        //    };
        //    _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(rooms);

        //    // Act
        //    var result = await _roomService.GetAvailableRooms();

        //    // Assert
        //    Assert.AreEqual(2, result.Count);
        //    Assert.IsTrue(result.All(r => r.Available));
        //}
    }
}
