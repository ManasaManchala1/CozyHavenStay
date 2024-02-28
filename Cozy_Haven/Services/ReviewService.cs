using Cozy_Haven.Exceptions;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Mappers;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<int, Review> _repository;

        public ReviewService(IRepository<int,Review> repository)
        {
            _repository=repository;
        }
        public async Task<Review> AddReview(ReviewDTO review)
        {
            Review newreview = new AddReview(review).GetReview();
            return await _repository.Add(newreview);
        }

        public async Task<Review> DeleteReview(int id)
        {
            var review=await GetReview(id);
            if(review!=null)
            {
                await _repository.Delete(id);
                return review;
            }
            throw new NoReviewFoundException();
        }

        public Task<List<Review>> GetAllReviews()
        {
            var reviews=_repository.GetAll();
            if(reviews!=null) return reviews;
            throw new NoReviewFoundException();
        }

        public Task<Review> GetReview(int id)
        {
            var review=_repository.GetById(id);
            if (review != null) return review;
            throw new NoReviewFoundException();
        }

        public async Task<Review> UpdateReviewRating(int id, float rating)
        {
            var hotel=await GetReview(id);
            if(hotel!=null)
            {
                hotel.Rating=rating;
                await _repository.Update(hotel);
                return hotel;
            }
            throw new NoReviewFoundException();
        }
    }
}
