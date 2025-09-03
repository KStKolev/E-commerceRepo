using System.ComponentModel.DataAnnotations;

namespace E_commerceApplication.DAL.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Platforms Platform { get; set; }

        public DateTime DateCreated { get; set; }

        public double TotalRating { get; set; }

        public decimal Price { get; set; }

    }
}
