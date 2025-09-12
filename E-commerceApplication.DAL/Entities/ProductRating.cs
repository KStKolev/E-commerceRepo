using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceApplication.DAL.Entities
{
    public class ProductRating
    {
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]

        public Product Product { get; set; } = null!;

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public ApplicationUser User { get; set; } = null!;

        public int Rating { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}