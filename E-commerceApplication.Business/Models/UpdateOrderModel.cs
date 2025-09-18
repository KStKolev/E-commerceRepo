namespace E_commerceApplication.Business.Models
{
    public class UpdateOrderModel
    {
        public Guid UserId { get; set; }

        public List<UpdateOrderItemModel> Items { get; set; } = new(); 
    }
}