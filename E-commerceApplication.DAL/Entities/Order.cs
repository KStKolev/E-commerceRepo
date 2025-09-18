using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerceApplication.DAL.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public ApplicationUser User { get; set; } = null!;

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}