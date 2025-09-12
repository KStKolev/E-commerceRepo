using E_commerceApplication.DTOs;

namespace E_commerceApplication.Tests.DTOTests
{
    public class DeleteRatingRequestDtoTests
    {
        [Fact]
        public void ProductIds_WithEmptyList_ShouldFailValidation()
        {
            var dto = new DeleteRatingRequestDto
            {
                ProductIds = new()
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .Single(results);

            Assert
                .Contains(results, r => r.MemberNames.Contains(nameof(DeleteRatingRequestDto.ProductIds)));
        }

        [Fact]
        public void ProductIds_WithOneItem_ShouldPassValidation()
        {
            int productId = 1;

            var dto = new DeleteRatingRequestDto
            {
                ProductIds = new List<int> { productId }
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .Empty(results);
        }

        [Fact]
        public void ProductIds_WithMultipleItems_ShouldPassValidation()
        {
            int productId1 = 1;
            int productId2 = 2;
            int productId3 = 3;

            var dto = new DeleteRatingRequestDto
            {
                ProductIds = new List<int> { productId1, productId2, productId3 }
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .Empty(results);
        }
    }
}