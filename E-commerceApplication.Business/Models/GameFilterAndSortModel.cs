using E_commerceApplication.DAL.Entities;

namespace E_commerceApplication.Business.Models
{
    public class GameFilterAndSortModel
    {
        public List<string> Genres { get; set; } = new();

        public Rating Age { get; set; }

        public SortByField SortBy { get; set; }

        public SortOrder SortOrder { get; set; }
    }
}