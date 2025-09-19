namespace E_commerceApplication.Business.Models
{
    public class AddOrderModel
    {
        public int Amount { get; set; } 

        public int ProductId { get; set; }

        public Guid UserId { get; set; }
    }
}
