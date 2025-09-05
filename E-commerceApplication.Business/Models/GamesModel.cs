using E_commerceApplication.DAL.Entities;

namespace E_commerceApplication.Business.Models
{
    public class GamesModel
    {
        public string Name { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        public Platforms Platform { get; set; }

        public string Logo { get; set; } = string.Empty;

        public string Background { get; set; } = string.Empty;

        public Rating Rating { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }
    }
}