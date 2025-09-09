using E_commerceApplication.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_commerceApplication.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Product>()
                .HasQueryFilter(p => !p.IsDeleted);

            builder
                .Entity<Product>()
                .HasIndex(p => p.Name);

            builder
                .Entity<Product>()
                .HasIndex(p => p.Platform);

            builder
                .Entity<Product>()
                .HasIndex(p => p.DateCreated);

            builder
                .Entity<Product>()
                .HasIndex(p => p.TotalRating);

            builder
                .Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder
                .Entity<Product>()
                .HasData(
                    new Product
                    {
                        Id = 1,
                        Name = "Halo Infinite",
                        Platform = Platforms.Web,
                        DateCreated = new DateTime(2021, 12, 8),
                        TotalRating = 20,
                        Price = 59.99m
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "God of War",
                        Platform = Platforms.Mobile,
                        DateCreated = new DateTime(2018, 4, 20),
                        TotalRating = 44,
                        Price = 39.99m
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Half-Life: Alyx",
                        Platform = Platforms.Web,
                        DateCreated = new DateTime(2020, 3, 23),
                        TotalRating = 59,
                        Price = 49.99m
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "The Legend of Zelda: Breath of the Wild",
                        Platform = Platforms.Desktop,
                        DateCreated = new DateTime(2017, 3, 3),
                        TotalRating = 72,
                        Price = 59.99m
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Elden Ring",
                        Platform = Platforms.Console,
                        DateCreated = new DateTime(2022, 2, 25),
                        TotalRating = 86,
                        Price = 69.99m
                    }
                );
        }
    }
}
