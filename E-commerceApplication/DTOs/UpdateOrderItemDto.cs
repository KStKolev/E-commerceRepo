using E_commerceApplication.Resources;
using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DTOs
{
    public class UpdateOrderItemDto
    {
        public int OrderItemId { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(OrderDtoValidationMessages),
            ErrorMessageResourceName = nameof(OrderDtoValidationMessages.InvalidAmount))]
        public int Amount { get; set; }
    }
}