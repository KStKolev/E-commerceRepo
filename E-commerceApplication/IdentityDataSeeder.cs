using E_commerceApplication.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_commerceApplication
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedIdentityAsync(UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager)
        {
            string[] roles = { "User", "Admin" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }

        }
    }
}