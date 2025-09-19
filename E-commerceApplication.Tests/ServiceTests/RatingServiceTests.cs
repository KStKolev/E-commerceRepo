using E_commerceApplication.Business.Models;
using E_commerceApplication.Business.Services;
using E_commerceApplication.DAL.Interfaces;
using Moq;

namespace E_commerceApplication.Tests.ServiceTests
{
    public class RatingServiceTests
    {
        private readonly Mock<IRatingRepository> _mockRatingRepository;
        private readonly Mock<IRatingValidationRepository> _mockRatingValidationRepository;
        private readonly RatingService _service;

        public RatingServiceTests()
        {
            _mockRatingRepository = new Mock<IRatingRepository>();
            _mockRatingValidationRepository = new Mock<IRatingValidationRepository>();
            _service = new RatingService(_mockRatingRepository.Object, _mockRatingValidationRepository.Object);
        }

        [Fact]
        public async Task EditRatingGameAsync_InvalidProductId_ReturnsFalse()
        {
            int testProductId = 1;
            int ratingValue = 5;

            var model = new EditRatingModel
            {
                ProductId = testProductId,
                UserId = Guid.NewGuid(),
                Rating = ratingValue
            };

            _mockRatingValidationRepository
                .Setup(r => r.CheckProductByIdAsync(model.ProductId))
                    .ReturnsAsync(false);

            var result = await _service
                .EditRatingGameAsync(model);

            Assert
                .False(result);
        }

        [Fact]
        public async Task EditRatingGameAsync_ValidInput_ReturnsTrue()
        {
            int testProductId = 1;
            int ratingValue = 5;

            var model = new EditRatingModel
            {
                ProductId = testProductId,
                UserId = Guid.NewGuid(),
                Rating = ratingValue
            };

            _mockRatingValidationRepository
                .Setup(r => r.CheckProductByIdAsync(model.ProductId))
                    .ReturnsAsync(true);

            _mockRatingRepository
                .Setup(r => r.EditRatingProductAsync(model.ProductId, model.UserId, model.Rating))
                    .Returns(Task.CompletedTask);

            var result = await _service
                .EditRatingGameAsync(model);

            Assert
                .True(result);

            _mockRatingRepository
                .Verify(r => r.EditRatingProductAsync(model.ProductId, model.UserId, model.Rating), Times.Once);
        }

        [Fact]
        public async Task DeleteRatingsAsync_ValidInput_ReturnsCompletedTask()
        {
            int productId1 = 1;
            int productId2 = 2;

            var model = new DeleteRatingModel
            {
                UserId = Guid.NewGuid(),
                ProductIds = new List<int> { productId1, productId2 }
            };

            _mockRatingRepository
                .Setup(r => r.DeleteRatingsAsync(model.UserId, model.ProductIds))
                    .Returns(Task.CompletedTask);

            await _service
                .DeleteRatingsAsync(model);

            _mockRatingRepository
                .Verify(r => r.DeleteRatingsAsync(model.UserId, model.ProductIds), Times.Once);
        }
    }
}