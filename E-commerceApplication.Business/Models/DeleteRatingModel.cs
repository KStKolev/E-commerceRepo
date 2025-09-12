namespace E_commerceApplication.Business.Models
{
    public class DeleteRatingModel
    {
        public Guid UserId { get; set; }

        public List<int> ProductIds { get; set; } = new();
    }
}
