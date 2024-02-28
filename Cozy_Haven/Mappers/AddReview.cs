using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Mappers
{
    public class AddReview
    {
        private readonly Review review;

        public AddReview(ReviewDTO addReviewDTO)
        {
            review = new Review
            {
                HotelId = addReviewDTO.HotelId,
                UserId = addReviewDTO.UserId,
                Rating = addReviewDTO.Rating,
                Comment = addReviewDTO.Comment,
                DatePosted = addReviewDTO.DatePosted
            };
        }

        public Review GetReview()
        {
            return review;
        }
    }
}
