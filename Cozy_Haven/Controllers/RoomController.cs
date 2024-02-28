using Cozy_Haven.Contexts;
using Cozy_Haven.Exceptions;
using Cozy_Haven.Helper;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;
using Cozy_Haven.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cozy_Haven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomservice;
        private readonly CozyHavenContext context;

        public RoomController(IRoomService roomService,CozyHavenContext _context)
        {
            _roomservice=roomService;
            context = _context;
        }
        [HttpGet("GetAllRooms")]
        public async Task<ActionResult<List<Room>>> GetRooms()
        {
            try
            {
                var rooms = await _roomservice.GetAllRooms();
                return Ok(rooms);
            }
            catch (NoRoomFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetById")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            try
            {
                var room = await _roomservice.GetRoom(id);
                return Ok(room);
            }
            catch (NoRoomFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("AddRoom")]
        public async Task<ActionResult<Room>> AddRoom(RoomDTO room)
        {
            try
            {
                var newroom=await _roomservice.AddRoom(room);
                return Ok(newroom);
               
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        [HttpPut("UpdatePrice")]
        public async Task<ActionResult<Room>> UpdatePrice(int id,int price)
        {
            try
            {
                var updatedRoom = await _roomservice.UpdateRoomPrice(id, price);
                return Ok(updatedRoom);
            }
            catch (NoRoomFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("DeleteRoom")]
        public async Task<ActionResult<Room>> DeleteRoom(int id)
        {
            try
            {
                var room = await _roomservice.DeleteRoom(id);
                return Ok(room);
            }
            catch (NoRoomFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetRoomAmenities")]
        public async Task<ActionResult<ICollection<RoomAmenity>>> GetRoomAmenities(int id)
        {
            try
            {
                var amenities = await _roomservice.GetRoomAmenities(id);
                return Ok(amenities);
            }
            catch (NoRoomFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NoAmenityFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        //[Authorize(Roles = "Admin,Owner")]
        [HttpPut("UpdateDetails")]
        public async Task<ActionResult<Room>> UpdateRoomDetails(Room room)
        {
            try
            {
                var updatedRoom = await _roomservice.UpdateRoomDetails(room);
                return Ok(updatedRoom);
            }
            catch (NoRoomFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        [HttpGet("AvailableRoomsCount")]
        public async Task<ActionResult<int>> GetAvailableRooms()
        {
            try
            {
                var availableRooms = await _roomservice.GetAvailableRoomsCount();
                return Ok(availableRooms);
            }
            catch (NoRoomFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception or return an error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("donut-chart-data")]
        public async Task<IActionResult> GetDonutChartData()
        {
            var data = await _roomservice.GetDonutChartData();
            return Ok(data);
        }
        [HttpPost("DBMultiUploadImage1")]
        public async Task<IActionResult> DBMultiUploadImage1(IFormFileCollection filecollection, int roomId)
        {
            APIResponse response = new APIResponse();
            int passcount = 0;
            int errorcount = 0;
            try
            {
                var existingImages = context.RoomImages.Where(image => image.RoomId == roomId).ToList();
                int currentImageCount = existingImages.Count;

                foreach (var file in filecollection)
                {
                    if (currentImageCount >= 5)
                    {
                        // Remove the image with the smallest ImageId (oldest image)
                        var oldestImage = existingImages.OrderBy(img => img.ImageId).First();
                        context.RoomImages.Remove(oldestImage);
                        existingImages.Remove(oldestImage);
                        currentImageCount--;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        var image = new Cozy_Haven.Models.RoomImage
                        {
                            ImagePath = stream.ToArray(),
                            RoomId = roomId
                        };
                        context.RoomImages.Add(image);
                        await context.SaveChangesAsync();
                        passcount++;
                        currentImageCount++;
                    }
                }

                response.ResponseCode = 200;
                response.Result = passcount + " Files uploaded & " + errorcount + " files failed";
            }
            catch (Exception ex)
            {
                errorcount++;
                response.Message = ex.Message;
                response.ResponseCode = 500;
            }
            return Ok(response);
        }
        [HttpGet("GetDBMultiImage2")]
        public async Task<IActionResult> GetDBMultiImage2(int roomId)
        {
            try
            {
                var images = await context.RoomImages.Where(item => item.RoomId == roomId).ToListAsync();
                if (images != null && images.Count() > 0)
                {
                    // Only return the first image for demonstration purposes
                    var image = images.First();

                    // Return the image data with appropriate content type
                    return File(image.ImagePath, "image/png");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpGet("GetDBMultiImage3")]
        //public async Task<IActionResult> GetDBMultiImage3(int roomId, int width, int height)
        //{
        //    try
        //    {
        //        var images = await context.RoomImages.Where(item => item.RoomId == roomId).ToListAsync();
        //        if (images != null && images.Count() > 0)
        //        {
        //            List<byte[]> resizedImages = new List<byte[]>();

        //            foreach (var image in images)
        //            {
        //                // Resize the image
        //                var resizedImage = ResizeImage(image.ImagePath, width, height);

        //                // Add the resized image to the list
        //                resizedImages.Add(resizedImage);
        //            }

        //            // Return the first image (you may modify this part according to your requirement)
        //            return File(resizedImages.First(), "image/png");
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
        //private byte[] ResizeImage(byte[] imageData, int width, int height)
        //{
        //    using (var ms = new MemoryStream(imageData))
        //    {
        //        using (var image = System.Drawing.Image.FromStream(ms))
        //        {
        //            using (var resizedImage = new Bitmap(width, height))
        //            {
        //                using (var graphics = Graphics.FromImage(resizedImage))
        //                {
        //                    graphics.CompositingQuality = CompositingQuality.HighQuality;
        //                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                    graphics.SmoothingMode = SmoothingMode.HighQuality;
        //                    graphics.DrawImage(image, 0, 0, width, height);
        //                }

        //                using (var output = new MemoryStream())
        //                {
        //                    resizedImage.Save(output, ImageFormat.Png);
        //                    return output.ToArray();
        //                }
        //            }
        //        }
        //    }
        //}
        [HttpDelete("DeleteImage/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            APIResponse response = new APIResponse();
            try
            {
                var imageToDelete = await context.RoomImages.FindAsync(imageId);
                if (imageToDelete != null)
                {
                    context.RoomImages.Remove(imageToDelete);
                    await context.SaveChangesAsync();
                    response.ResponseCode = 200;
                    response.Message = "Image deleted successfully";
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Message = "Image not found";
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = "An error occurred while deleting the image: " + ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete("DeleteImagesByRoom/{roomId}")]
        public async Task<IActionResult> DeleteImagesByHotel(int roomId)
        {
            APIResponse response = new APIResponse();
            try
            {
                var imagesToDelete = context.RoomImages.Where(image => image.RoomId == roomId).ToList();
                foreach (var image in imagesToDelete)
                {
                    context.RoomImages.Remove(image);
                }
                await context.SaveChangesAsync();
                response.ResponseCode = 200;
                response.Message = "All images for hotel with ID " + roomId + " deleted successfully";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500;
                response.Message = "An error occurred while deleting images: " + ex.Message;
            }
            return Ok(response);
        }


    }
}
