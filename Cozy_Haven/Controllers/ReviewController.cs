using Cozy_Haven.Exceptions;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cozy_Haven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewservice;

        public ReviewController(IReviewService reviewService)
        {
            _reviewservice=reviewService;
        }
        [HttpGet("AllReviews")]
        public async Task<ActionResult<List<Review>>> GetReviews()
        {
            try
            {
                return await _reviewservice.GetAllReviews();
            }
            catch (NoReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("GetById")]
        public async Task<ActionResult<Review>> GetReviewById(int id)
        {
            try
            {
                return await _reviewservice.GetReview(id);
            }
            catch (NoReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        //[HttpGet("GetByHotelId")]
        //public async Task<List<Review>> GetHotelReviews(int id)
        //{
        //    return await _reviewservice.GetReviewsByHotelId(id);
        //}
        //[HttpGet("GetByUserId")]
        //public async Task<List<Review>> GetUserReviews(int id)
        //{
        //    return await _reviewservice.GetReviewsByUserId(id);
        //}
        [HttpPost("AddReview")]
        public async Task<ActionResult<Review>> AddReview(ReviewDTO review)
        {
            try
            {
                return await _reviewservice.AddReview(review);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPut("UpdateRating")]
        public async Task<ActionResult<Review>> UpdateRating(int id,float rating)
        {
            try
            {
                return await _reviewservice.UpdateReviewRating(id, rating);
            }
            catch (NoReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        [HttpDelete("DeleteReview")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            try
            {
                return await _reviewservice.DeleteReview(id);
            }
            catch (NoReviewFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
