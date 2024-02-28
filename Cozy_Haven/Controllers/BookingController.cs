using Cozy_Haven.Exceptions;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;
using Cozy_Haven.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cozy_Haven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingservice;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingService bookingService,ILogger<BookingController> logger) {
            _bookingservice=bookingService;
            _logger=logger;
        }
        [HttpGet("AllBookings")]
        public async Task<ActionResult<List<Booking>>> GetBookings()
        {
            try
            {
                return await _bookingservice.GetAllBookings();
            }
            catch (NoBookingFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("GetById")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            try
            {
                return await _bookingservice.GetBooking(id);
            }
            catch (NoBookingFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        //[HttpPost("AddBooking")]
        //public async Task<ActionResult<Booking>> AddBooking(BookingDTO booking)
        //{
        //    try
        //    {
        //        return await _bookingservice.AddBooking(booking);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        //    }
        //}
        [HttpPut("UpdateBookingStatus")]
        public async Task<ActionResult<Booking>> UpdateBooking(int id,string status)
        {
            try
            {
                return await _bookingservice.UpdateBookingStatus(id, status);
            }
            catch (NoBookingFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [HttpDelete("DeleteBooking")]
        public async Task<ActionResult<Booking>> DeleteBooking(int id){
            try
            {
                var booking=await _bookingservice.DeleteBooking(id);
                return Ok(booking);
            }
            catch (NoBookingFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("HotelBookings")]
        public async Task<IActionResult> GetHotelBookings(int hotelId)
        {
            try
            {
                var bookings = await _bookingservice.GetHotelBookings(hotelId);
                return Ok(bookings);

            }
            catch (NoBookingFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("CheckAvailability")]
        public async Task<ActionResult<bool>> CheckRoomAvailability(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            bool isRoomAvailable = await _bookingservice.IsRoomAvailable(roomId, checkInDate, checkOutDate);
            return Ok(isRoomAvailable);
        }

        [HttpPost("AddBooking")]
        public async Task<ActionResult<Booking>> AddBooking(BookingDTO booking, string username)
        {
            try
            {
                var addedBooking = await _bookingservice.AddBooking(booking, username);
                //return CreatedAtAction(nameof(GetBooking), new { id = addedBooking.BookingId }, addedBooking);
                return Ok(addedBooking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("TotalBookings")]
        public async Task<ActionResult<int>> GetTotalBookings()
        {
            try
            {
                var totalBookings = await _bookingservice.GetTotalBookings();
                return Ok(totalBookings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("TotalRevenue")]
        public async Task<ActionResult<float>> GetTotalRevenue()
        {
            try
            {
                var totalRevenue = await _bookingservice.GetTotalRevenue();
                return Ok(totalRevenue);
            }
            catch (NoBookingFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("line-chart-data")]
        public async Task<IActionResult> GetLineChartData()
        {
            var data = await _bookingservice.GetLineChartData();

            return Ok(data);
        }
        [HttpPost("CancelBooking/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var booking = await _bookingservice.GetBooking(bookingId);

            if (booking == null)
            {
                return NotFound("Booking not found");
            }

            // Cancel the booking
            var cancelResult = await _bookingservice.CancelBooking(bookingId);

            if (cancelResult)
            {
                return Ok("Booking cancelled successfully");
            }
            else
            {
                return StatusCode(500, "Failed to cancel booking or process refund");
            }
        }
        [HttpGet("Cancelledbookings")]
        public async Task<IActionResult> GetCancelledHotelBookings(int hotelId)
        {
            try
            {
                var bookings = await _bookingservice.GetCancelledHotelBookings(hotelId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cancelled bookings for hotel {HotelId}", hotelId);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("UpdateBooking")]
        public IActionResult UpdateBooking(int id,Booking updatedBooking)
        {
            try
            {
                var booking=_bookingservice.UpdateBooking(id, updatedBooking);
                return Ok(booking);
            }
            catch (NoBookingFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }






    }
}
