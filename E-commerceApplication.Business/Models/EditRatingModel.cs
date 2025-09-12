namespace E_commerceApplication.Business.Models
{
    public class EditRatingModel
    {
        public int ProductId { get; set; }

        public Guid UserId { get; set; }

        public int Rating { get; set; }
    }
}