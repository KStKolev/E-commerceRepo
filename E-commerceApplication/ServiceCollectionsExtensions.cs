using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Services;
using E_commerceApplication.DAL.Data;
using E_commerceApplication.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_commerceApplication
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddScoped<IAuthService, AuthService>();
            services
                .AddScoped<IEmailService, EmailService>();
            services
                .AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
