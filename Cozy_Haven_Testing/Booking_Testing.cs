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
    public class Booking_Testing
    {
        private IBookingService _bookingService;
        private Mock<IRepository<int,Booking>> _mockBookingRepo;
        private Mock<IRepository<int, Room>> _mockRoomRepo;
        private Mock<IRepository<string, User>> _mockUserRepo;
        private Mock<IPaymentService> _mockPaymentRepo;
        private Mock<ILogger<BookingService>> _logger;


        [SetUp]
        public void Setup()
        {
            _mockBookingRepo = new Mock<IRepository<int, Booking>>();
            _mockRoomRepo = new Mock<IRepository<int, Room>>();
            _mockUserRepo=new Mock<IRepository<string, User>>();
            _mockPaymentRepo=new Mock<IPaymentService>();
            _logger = new Mock<ILogger<BookingService>>();
            _bookingService = new BookingService(_mockBookingRepo.Object, _mockRoomRepo.Object, _mockUserRepo.Object, _mockPaymentRepo.Object, _logger.Object);
        }

        //[Test]
        //public async Task AddBookingSuccessTest()
        //{
        //    // Arrange
        //    var bookingDto = new BookingDTO { UserId = 1, RoomId = 1 };
        //    var booking = new Booking { BookingId = 1, UserId = 1, RoomId = 1 };

        //    _mockBookingRepo.Setup(x => x.Add(It.IsAny<Booking>())).ReturnsAsync(booking);

        //    // Act
        //    var result = await _bookingService.AddBooking(bookingDto,"Sheldon7");

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.BookingId);
        //    Assert.AreEqual(1, result.UserId);
        //    Assert.AreEqual(1, result.RoomId);
        //}

        [Test]
        public async Task DeleteBookingSuccessTest()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { BookingId = bookingId, UserId = 1, RoomId = 1 };
            _mockBookingRepo.Setup(x => x.GetById(bookingId)).ReturnsAsync(booking);
            _mockBookingRepo.Setup(x => x.Delete(bookingId)).ReturnsAsync(booking);

            // Act
            var result = await _bookingService.DeleteBooking(bookingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookingId, result.BookingId);
        }

        [Test]
        public async Task GetAllBookingsSuccessTest()
        {
            // Arrange
            var bookings = new List<Booking> { new Booking { BookingId = 1 }, new Booking { BookingId = 2 } };
            _mockBookingRepo.Setup(x => x.GetAll()).ReturnsAsync(bookings);

            // Act
            var result = await _bookingService.GetAllBookings();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetBookingSuccessTest()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { BookingId = bookingId };
            _mockBookingRepo.Setup(x => x.GetById(bookingId)).ReturnsAsync(booking);

            // Act
            var result = await _bookingService.GetBooking(bookingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookingId, result.BookingId);
        }

        [Test]
        public void GetBookingNotFoundExceptionTest()
        {
            // Arrange
            var bookingId = 1;
            _mockBookingRepo.Setup(x => x.GetById(bookingId)).ReturnsAsync((Booking)null);

            // Act & Assert
            Assert.ThrowsAsync<NoBookingFoundException>(async () => await _bookingService.GetBooking(bookingId));
        }

        [Test]
        public async Task GetBookingsCountSuccessTest()
        {
            // Arrange
            var bookings = new List<Booking> { new Booking(), new Booking(), new Booking() };
            _mockBookingRepo.Setup(x => x.GetAll()).ReturnsAsync(bookings);

            // Act
            var result = await _bookingService.GetBookingsCount();

            // Assert
            Assert.AreEqual(3, result);
        }

        [Test]
        public async Task UpdateBookingStatusSuccessTest()
        {
            // Arrange
            var bookingId = 1;
            var status = "CheckedOut";
            var booking = new Booking { BookingId = bookingId, Status = "Booked", Room = new Room { Available = false } };
            _mockBookingRepo.Setup(x => x.GetById(bookingId)).ReturnsAsync(booking);
            _mockBookingRepo.Setup(x => x.Update(It.IsAny<Booking>())).ReturnsAsync((Booking b) => b);

            // Act
            var result = await _bookingService.UpdateBookingStatus(bookingId, status);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookingId, result.BookingId);
            Assert.AreEqual(status, result.Status);
            Assert.IsTrue(result.Room.Available);
        }

        [Test]
        public async Task GetHotelBookingsSuccessTest()
        {
            // Arrange
            var hotelId = 1;
            var rooms = new List<Room>
            {
                new Room { RoomId = 1, HotelId = hotelId },
                new Room { RoomId = 2, HotelId = hotelId },
                new Room { RoomId = 3, HotelId = hotelId }
            };
            var bookings = new List<Booking>
            {
                new Booking { RoomId = 1 },
                new Booking { RoomId = 2 },
                new Booking { RoomId = 3 },
                new Booking { RoomId = 4 }
            };
            _mockRoomRepo.Setup(x => x.GetAll()).ReturnsAsync(rooms);
            _mockBookingRepo.Setup(x => x.GetAll()).ReturnsAsync(bookings);

            // Act
            var result = await _bookingService.GetHotelBookings(hotelId);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.All(b => rooms.Select(r => r.RoomId).Contains(b.RoomId)));
        }
    }
}
