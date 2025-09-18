namespace E_commerceApplication.DTOs
{
    public class UpdateOrderDto
    {
        public List<UpdateOrderItemDto> Items { get; set; } = new();
    }
}