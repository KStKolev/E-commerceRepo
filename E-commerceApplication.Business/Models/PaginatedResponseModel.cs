namespace E_commerceApplication.Business.Models
{
    public class PaginatedResponseModel<T>
    {
        public List<T> Items { get; set; } = new();

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}