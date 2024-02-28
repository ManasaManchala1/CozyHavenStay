using Cozy_Haven.Exceptions;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;
using Cozy_Haven.Services;
using Moq;
using NUnit.Framework;

namespace Cozy_Haven_Testing
{
    public class ReviewServiceTesting
    {
        private IReviewService _reviewService;
        private Mock<IRepository<int, Review>> _mockRepo;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<int, Review>>();
            _reviewService = new ReviewService(_mockRepo.Object);
        }

        [Test]
        public async Task AddReviewSuccessTest()
        {
            // Arrange
            var reviewDto = new ReviewDTO { Rating = 4.5f, Comment = "Great hotel!" };
            var review = new Review { ReviewId = 1, Rating = 4.5f, Comment = "Great hotel!" };

            _mockRepo.Setup(x => x.Add(It.IsAny<Review>())).ReturnsAsync(review);

            // Act
            var result = await _reviewService.AddReview(reviewDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ReviewId);
            Assert.AreEqual(4.5f, result.Rating);
            Assert.AreEqual("Great hotel!", result.Comment);
        }

        [Test]
        public async Task DeleteReviewSuccessTest()
        {
            // Arrange
            var reviewId = 1;
            var review = new Review { ReviewId = reviewId, Rating = 4.5f, Comment = "Great hotel!" };
            _mockRepo.Setup(x => x.GetById(reviewId)).ReturnsAsync(review);
            _mockRepo.Setup(x => x.Delete(reviewId)).ReturnsAsync(review);

            // Act
            var result = await _reviewService.DeleteReview(reviewId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reviewId, result.ReviewId);
            Assert.AreEqual(4.5f, result.Rating);
            Assert.AreEqual("Great hotel!", result.Comment);
        }

        [Test]
        public async Task GetAllReviewsSuccessTest()
        {
            // Arrange
            var reviews = new List<Review> { new Review { ReviewId = 1 }, new Review { ReviewId = 2 } };
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(reviews);

            // Act
            var result = await _reviewService.GetAllReviews();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetReviewSuccessTest()
        {
            // Arrange
            var reviewId = 1;
            var review = new Review { ReviewId = reviewId };
            _mockRepo.Setup(x => x.GetById(reviewId)).ReturnsAsync(review);

            // Act
            var result = await _reviewService.GetReview(reviewId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reviewId, result.ReviewId);
        }

        [Test]
        public async Task UpdateReviewRatingSuccessTest()
        {
            // Arrange
            var reviewId = 1;
            var newRating = 4.0f;
            var review = new Review { ReviewId = reviewId, Rating = 3.5f };
            _mockRepo.Setup(x => x.GetById(reviewId)).ReturnsAsync(review);
            _mockRepo.Setup(x => x.Update(It.IsAny<Review>())).ReturnsAsync((Review r) => r);

            // Act
            var result = await _reviewService.UpdateReviewRating(reviewId, newRating);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(reviewId, result.ReviewId);
            Assert.AreEqual(newRating, result.Rating);
        }
    }
}
