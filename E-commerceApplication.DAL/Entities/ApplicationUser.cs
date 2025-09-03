using Microsoft.AspNetCore.Identity;

namespace E_commerceApplication.DAL.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string AddressDelivery { get; set; } = string.Empty;
    }
}