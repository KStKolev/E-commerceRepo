using Microsoft.AspNetCore.Identity;

namespace E_commerceApplication.DAL.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string AddressDelivery { get; set; } = string.Empty;

        public ICollection<ProductRating> Ratings { get; set; } = 
            new List<ProductRating>();

        public ICollection<Order> Orders { get; set; } =
            new List<Order>();
    }
}