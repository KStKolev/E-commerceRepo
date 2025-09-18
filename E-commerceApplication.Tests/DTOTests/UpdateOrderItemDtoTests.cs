using E_commerceApplication.DTOs;
using E_commerceApplication.Resources;

namespace E_commerceApplication.Tests.DTOTests
{
    public class UpdateOrderItemDtoTests
    {
        [Fact]
        public void Amount_WhenLessThanOne_ShouldBeInvalid()
        {
            int orderItemId = 1;
            int amount = 0;

            var dto = new UpdateOrderItemDto
            {
                OrderItemId = orderItemId,
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
            int orderItemItemId = 1;
            int amount = 1;

            var dto = new UpdateOrderItemDto
            {
                OrderItemId = orderItemItemId,
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
            int orderItemId = 1;
            int amount = 10;

            var dto = new UpdateOrderItemDto
            {
                OrderItemId = orderItemId,
                Amount = amount
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .Empty(results);
        }
    }
}