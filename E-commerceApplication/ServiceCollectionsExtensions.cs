using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Services;
using E_commerceApplication.DAL.Data;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;
using E_commerceApplication.DAL.Repositories;
using E_commerceApplication.Validation;
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
            services
                .AddScoped<IProductRepository, ProductRepository>();
            services
                .AddScoped<IGamesService, GamesService>();
            services
                .AddScoped<IImageService, ImageService>();
            services
                .AddScoped<IRatingService, RatingService>();
            services
                .AddScoped<IRatingRepository, RatingRepository>();
            services
                .AddScoped<IRatingValidationRepository, RatingValidationRepository>();
            services
                .AddScoped<IOrdersService, OrdersService>();
            services
                .AddScoped<IOrdersRepository, OrdersRepository>();
            services
                .AddScoped<IOrdersValidationRepository, OrdersValidationRepository>();

            services
                .AddScoped<ValidateGameListParamsAttribute>();

            return services;
        }
    }
}