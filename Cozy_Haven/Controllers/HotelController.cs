using Cozy_Haven.Contexts;
using Cozy_Haven.Exceptions;
using Cozy_Haven.Helper;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Cozy_Haven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly CozyHavenContext context;

        public HotelController(IHotelService hotelService,CozyHavenContext _context)
        {
            _hotelService = hotelService;
            context=_context;
        }

        [HttpGet("GetAllHotels")]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _hotelService.GetAllHotels();
                return Ok(hotels);
            }
            catch (NoHotelFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var hotel = await _hotelService.GetHotel(id);
                return Ok(hotel);
            }
            catch (NoHotelFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet("GetByDestinationId")]
        //public async Task<IActionResult> GetByDestinationId(int id)
        //{
        //    try
        //    {
        //        var hotels = await _hotelService.GetHotelsByDestinationId(id);
        //        return Ok(hotels);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        [HttpPost("AddHotel")]
        public async Task<IActionResult> AddHotel(HotelDTO hotel)
        {
            try
            {
                var addedHotel = await _hotelService.AddHotel(hotel);
                return Ok(addedHotel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdateDescription")]
        public async Task<IActionResult> UpdateDescription(int id, string description)
        {
            try
            {
                var updatedHotel = await _hotelService.UpdateHotelDescription(id, description);
                return Ok(updatedHotel);
            }
            catch (NoHotelFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteHotel")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
                var deletedHotel = await _hotelService.DeleteHotel(id);
                return Ok(deletedHotel);
            }
            catch (NoHotelFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("UpdateHotelOwner")]
        public async Task<IActionResult> UpdateHotelOwner(int id, int ownerId)
        {
            try
            {
                var updatedHotel = await _hotelService.UpdateHotelOwner(id, ownerId);
                return Ok(updatedHotel);
            }
            catch (NoHotelFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("HotelReviews")]
        public async Task<IActionResult> GetHotelReviews(int id)
        {
            try
            {
                var reviews = await _hotelService.GetHotelReviews(id);
                return Ok(reviews);
            }
            catch (NoHotelFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("HotelAmenities")]
        public async Task<IActionResult> GetHotelAmenities(int id)
        {
            try
            {
                var amenities = await _hotelService.GetHotelAmenities(id);
                return Ok(amenities);
            }
            catch (NoHotelFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("RecommendedHotels")]
        public async Task<ActionResult<ICollection<Hotel>>> GetRecommendedHotels()
        {
            try
            {
                var recommendedHotels = await _hotelService.GetRecommendedHotels();
                return Ok(recommendedHotels);
            }
            catch
            {
                return StatusCode(500, "Failed to retrieve recommended hotels.");
            }
        }
        [HttpGet("GetRoomsByHotelId")]
        public async Task<ActionResult<ICollection<Room>>> GetRoomsByHotelId(int hotelId)
        {
            try
            {
                var rooms = await _hotelService.GetRoomsByHotelId(hotelId);
                return Ok(rooms);
            }
            catch (NoHotelFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("UpdateHotelDetails")]
        public async Task<IActionResult> UpdateHotelDetails(Hotel hotel)
        {
            try
            {
                var updatedHotel = await _hotelService.UpdateHotelDetails(hotel);
                return Ok(updatedHotel);
            }
            catch (NoHotelFoundException ex)
            { 
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("GetHotelBookings")]
        public async Task<IActionResult> GetHotelBookings(int hotelId)
        {
            try
            {
                var bookings = await _hotelService.GetHotelBookings(hotelId);
                return Ok(bookings);
            }
            catch (NoHotelFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
        [HttpGet("OwnedHotels")]
        public async Task<IActionResult> GetHotelsByOwnerId(int ownerId)
        {
            try
            {
                var hotels = await _hotelService.GetHotelsByOwnerId(ownerId);
                return Ok(hotels);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while retrieving hotels.");
            }
        }
        [HttpPut("DBMultiUploadImage")]
        public async Task<IActionResult> DBMultiUploadImage(IFormFileCollection filecollection, int hotelId)
        {
            APIResponse response = new APIResponse();
            int passcount = 0; int errorcount = 0;
            try
            {
                foreach (var file in filecollection)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        var image = new Cozy_Haven.Models.HotelImage
                        {
                            ImagePath = stream.ToArray(),
                            HotelId = hotelId,
                        };
                        context.HotelImages.Add(image);
                        await context.SaveChangesAsync();
                        passcount++;
                    }
                }


            }
            catch (Exception ex)
            {
                errorcount++;
                response.Message = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passcount + " Files uploaded &" + errorcount + " files failed";
            return Ok(response);
        }

        [HttpPost("DBMultiUploadImage1")]
        public async Task<IActionResult> DBMultiUploadImage1(IFormFileCollection filecollection, int hotelId)
        {
            APIResponse response = new APIResponse();
            int passcount = 0;
            int errorcount = 0;
            try
            {
                var existingImages = context.HotelImages.Where(image => image.HotelId == hotelId).ToList();
                int currentImageCount = existingImages.Count;

                foreach (var file in filecollection)
                {
                    if (currentImageCount >= 5)
                    {
                        //Replacing with imageid -->smallest
                        var oldestImage = existingImages.OrderBy(img => img.ImageId).First();
                        context.HotelImages.Remove(oldestImage);
                        existingImages.Remove(oldestImage);
                        currentImageCount--;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        var image = new Cozy_Haven.Models.HotelImage
                        {
                            ImagePath = stream.ToArray(),
                            HotelId = hotelId
                        };
                        context.HotelImages.Add(image);
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





        [HttpGet("GetDBMultiImage")]
        public async Task<IActionResult> GetDBMultiImage(int hotelId)
        {
            List<string> imageUrls = new List<string>();
            try
            {
                var images = await context.HotelImages.Where(item => item.HotelId == hotelId).ToListAsync();
                if (images != null && images.Count() > 0)
                {
                    imageUrls = images.Select(item => Convert.ToBase64String(item.ImagePath)).ToList();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
            return Ok(imageUrls);
        }


        [HttpGet("GetDBMultiImage2")]
        public async Task<IActionResult> GetDBMultiImage2(int hotelId)
        {
            try
            {
                var images = await context.HotelImages.Where(item => item.HotelId == hotelId).ToListAsync();
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
        //public async Task<IActionResult> GetDBMultiImage3(int hotelId, int width, int height)
        //{
        //    try
        //    {
        //        var images = await context.HotelImages.Where(item => item.HotelId == hotelId).ToListAsync();
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


        [HttpGet("dbdownload")]
        public async Task<IActionResult> dbdownload(int hotelId)
        {
            try
            {
                var image = await context.HotelImages.FirstOrDefaultAsync(item => item.HotelId == hotelId);
                if (image != null)
                {
                    return File(image.ImagePath, "image/png", hotelId + ".png");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return NotFound();
            }
        }
        [HttpGet("dbdownload1")]
        public async Task<IActionResult> dbdownload1(int hotelId)
        {
            try
            {
                var images = await context.HotelImages.Where(item => item.HotelId == hotelId).ToListAsync();
                if (images != null && images.Count > 0)
                {
                    // Create a memory stream to hold the zip file content
                    var memoryStream = new MemoryStream();

                    // Create a ZipArchive to write the images to
                    using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var image in images)
                        {
                            // Add each image to the zip archive
                            var entry = zipArchive.CreateEntry($"{image.ImageId}.png");
                            using (var entryStream = entry.Open())
                            {
                                await entryStream.WriteAsync(image.ImagePath, 0, image.ImagePath.Length);
                            }
                        }
                    }

                    // Reset the memory stream position to the beginning
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Return the zip file as a FileStreamResult
                    return File(memoryStream, "application/zip", $"{hotelId}_images.zip");
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

        [HttpDelete("DeleteImage/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            APIResponse response = new APIResponse();
            try
            {
                var imageToDelete = await context.HotelImages.FindAsync(imageId);
                if (imageToDelete != null)
                {
                    context.HotelImages.Remove(imageToDelete);
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

        [HttpDelete("DeleteImagesByHotel/{hotelId}")]
        public async Task<IActionResult> DeleteImagesByHotel(int hotelId)
        {
            APIResponse response = new APIResponse();
            try
            {
                var imagesToDelete = context.HotelImages.Where(image => image.HotelId == hotelId).ToList();
                foreach (var image in imagesToDelete)
                {
                    context.HotelImages.Remove(image);
                }
                await context.SaveChangesAsync();
                response.ResponseCode = 200;
                response.Message = "All images for hotel with ID " + hotelId + " deleted successfully";
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
