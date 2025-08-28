using E_commerceApplication.Business;
using E_commerceApplication.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_commerceApplication
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedIdentityAsync(UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager)
        {
            foreach (RoleType role in Enum.GetValues(typeof(RoleType)))
            {
                string roleName = role.ToString();
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }

        }
    }
}