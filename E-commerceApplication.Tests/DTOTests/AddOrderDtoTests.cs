using E_commerceApplication.DTOs;
using E_commerceApplication.Resources;

namespace E_commerceApplication.Tests.DTOTests
{
    public class AddOrderDtoTests
    {
        [Fact]
        public void Amount_WhenLessThanOne_ShouldBeInvalid()
        {
            int productId = 5;
            int amount = 0;

            var dto = new AddOrderRequestDto
            {
                ProductId = productId,
                Amount = amount
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .NotEmpty(results);

            Assert.Contains(results, r => r.ErrorMessage == OrderDtoValidationMessages.InvalidAmount);
        }

        [Fact]
        public void Amount_WhenEqualToOne_ShouldBeValid()
        {
            int productId = 5;
            int amount = 1;

            var dto = new AddOrderRequestDto
            {
                ProductId = productId,
                Amount = amount
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .Empty(results);
        }

        [Fact]
        public void Amount_WhenGreaterThanOne_ShouldBeValid()
        {
            int productId = 5;
            int amount = 42;

            var dto = new AddOrderRequestDto
            {
                ProductId = productId,
                Amount = amount
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .Empty(results);
        }
    }
}