using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DAL.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        public Platforms Platform { get; set; }

        public Rating Rating { get; set; }

        public string Logo { get; set; } = string.Empty;

        public string Background { get; set; } = string.Empty;

        public int Count { get; set; }

        public DateTime DateCreated { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; } = false;

        public double TotalRating { get; set; }

        public ICollection<ProductRating> Ratings { get; set; } = 
            new List<ProductRating>();
    }
}