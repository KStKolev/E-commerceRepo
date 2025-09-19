namespace E_commerceApplication.Business.Models
{
    public class DeleteItemsModel
    {
        public List<int> ItemsIdList { get; set; } = new();

        public Guid UserId { get; set; }
    }
}
