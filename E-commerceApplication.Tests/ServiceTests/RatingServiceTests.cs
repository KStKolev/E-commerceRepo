using E_commerceApplication.Business.Models;
using E_commerceApplication.Business.Services;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;
using Moq;

namespace E_commerceApplication.Tests.ServiceTests
{
    public class RatingServiceTests
    {
        private readonly Mock<IRatingRepository> _mockRatingRepository;
        private readonly RatingService _service;

        public RatingServiceTests()
        {
            _mockRatingRepository = new Mock<IRatingRepository>();
            _service = new RatingService(_mockRatingRepository.Object);
        }

        [Fact]
        public async Task EditRatingGameAsync_ProductOrUserIsNull_ReturnsFalse()
        {
            int testProductId = 1;
            int ratingValue = 5;

            var model = new EditRatingModel
            {
                ProductId = testProductId,
                UserId = Guid.NewGuid(),
                Rating = ratingValue
            };

            _mockRatingRepository
                .Setup(r => r.GetProductWithRatingsAsync(model.ProductId))
                .ReturnsAsync((Product?)null);

            _mockRatingRepository
                .Setup(r => r.GetUserWithRatingsAsync(model.UserId))
                .ReturnsAsync((ApplicationUser?)null);

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

            var product = new Product { Id = testProductId };
            var user = new ApplicationUser { Id = model.UserId };

            _mockRatingRepository
                .Setup(r => r.GetProductWithRatingsAsync(model.ProductId))
                .ReturnsAsync(product);

            _mockRatingRepository
                .Setup(r => r.GetUserWithRatingsAsync(model.UserId))
                .ReturnsAsync(user);

            _mockRatingRepository
                .Setup(r => r.EditRatingProductAsync(product, user, model.Rating))
                .Returns(Task.CompletedTask);

            var result = await _service
                .EditRatingGameAsync(model);

            Assert
                .True(result);

            _mockRatingRepository
                .Verify(r => r.EditRatingProductAsync(product, user, model.Rating), Times.Once);
        }

        [Fact]
        public async Task DeleteRatingsAsync_UserIsNull_ReturnsFalse()
        {
            int productId1 = 1;
            int productId2 = 2;
            int productId3 = 3;

            var model = new DeleteRatingModel
            {
                UserId = Guid.NewGuid(),
                ProductIds = new List<int> { productId1, productId2, productId3 }
            };

            _mockRatingRepository
                .Setup(r => r.GetUserWithRatingsAsync(model.UserId))
                .ReturnsAsync((ApplicationUser?)null);

            var result = await _service
                .DeleteRatingsAsync(model);

            Assert
                .False(result);
        }

        [Fact]
        public async Task DeleteRatingsAsync_ProductIsNull_ReturnsFalse()
        {
            int productId1 = 1;
            int productId2 = 2;
            int productId3 = 3;

            var model = new DeleteRatingModel
            {
                UserId = Guid.NewGuid(),
                ProductIds = new List<int> { productId1, productId2, productId3 }
            };

            var user = new ApplicationUser { Id = model.UserId };

            _mockRatingRepository
                .Setup(r => r.GetUserWithRatingsAsync(model.UserId))
                .ReturnsAsync(user);

            _mockRatingRepository
                .Setup(r => r.GetProductWithRatingsAsync(productId1))
                .ReturnsAsync((Product?)null);

            var result = await _service
                .DeleteRatingsAsync(model);

            Assert
                .False(result);
        }

        [Fact]
        public async Task DeleteRatingsAsync_ValidInput_ReturnsTrue()
        {
            int productId1 = 1;
            int productId2 = 2;
            int productIndex1 = 0;
            int productIndex2 = 1;

            var model = new DeleteRatingModel
            {
                UserId = Guid.NewGuid(),
                ProductIds = new List<int> { productId1, productId2 }
            };

            var user = new ApplicationUser { Id = model.UserId };

            var products = new List<Product>
            {
                new Product { Id = productId1 },
                new Product { Id = productId2 },
            };

            _mockRatingRepository
                .Setup(r => r.GetUserWithRatingsAsync(model.UserId))
                .ReturnsAsync(user);

            _mockRatingRepository
                .Setup(r => r.GetProductWithRatingsAsync(productId1))
                .ReturnsAsync(products[productIndex1]);

            _mockRatingRepository
                .Setup(r => r.GetProductWithRatingsAsync(productId2))
                .ReturnsAsync(products[productIndex2]);

            _mockRatingRepository
                .Setup(r => r.DeleteRatingsAsync(user, products))
                .Returns(Task.CompletedTask);

            var result = await _service
                .DeleteRatingsAsync(model);

            Assert
                .True(result);

            _mockRatingRepository
                .Verify(r => r.DeleteRatingsAsync(user, products), Times.Once);
        }
    }
}