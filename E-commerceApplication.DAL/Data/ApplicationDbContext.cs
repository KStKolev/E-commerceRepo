using E_commerceApplication.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace E_commerceApplication.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductRating> ProductRatings { get; set; }

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
                .HasIndex(p => p.Genre);

            builder
                .Entity<Product>()
                .HasIndex(p => p.Platform);

            builder
                .Entity<Product>()
                .HasIndex(p => p.Rating);

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
                .Entity<ProductRating>()
                .HasKey(pr => new { pr.ProductId, pr.UserId });

            builder
                .Entity<ProductRating>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.Ratings);

            builder
                .Entity<ProductRating>()
                .HasOne(pr => pr.User)
                .WithMany(u => u.Ratings);

            builder
                .Entity<Product>()
                .HasData(
                    new Product
                    {
                        Id = 1,
                        Name = "Halo Infinite",
                        Genre = "Shooter",
                        Platform = Platforms.Web,
                        DateCreated = new DateTime(2021, 12, 8),
                        TotalRating = 0,
                        Price = 59.99m
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "God of War",
                        Genre = "Action",
                        Platform = Platforms.Mobile,
                        DateCreated = new DateTime(2018, 4, 20),
                        TotalRating = 0,
                        Price = 39.99m
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Half-Life: Alyx",
                        Genre = "Shooter",
                        Platform = Platforms.Web,
                        DateCreated = new DateTime(2020, 3, 23),
                        TotalRating = 0,
                        Price = 49.99m
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "The Legend of Zelda: Breath of the Wild",
                        Genre = "Adventure",
                        Platform = Platforms.Desktop,
                        DateCreated = new DateTime(2017, 3, 3),
                        TotalRating = 0,
                        Price = 59.99m
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Elden Ring",
                        Genre = "RPG",
                        Platform = Platforms.Console,
                        DateCreated = new DateTime(2022, 2, 25),
                        TotalRating = 0,
                        Price = 69.99m
                    }
                );
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateProductTotalRating();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateProductTotalRating()
        {
            List<EntityEntry<ProductRating>> changed = ChangeTracker
                .Entries<ProductRating>()
                .Where(e =>
                    e.State == EntityState.Added 
                    || e.State == EntityState.Modified
                    || e.State == EntityState.Deleted)
                .ToList();

            foreach (EntityEntry<ProductRating> entry in changed)
            {
                int productId = entry.Entity.ProductId;
                Product? product = Products
                    .Find(productId);

                if (product == null)
                {
                    continue;
                }

                List<int> dbRatings = ProductRatings
                    .Where(pr => pr.ProductId == productId)
                    .Select(pr => pr.Rating)
                    .ToList();

                switch (entry.State)
                {
                    case EntityState.Added:
                        dbRatings.Add(entry.Entity.Rating);
                        break;
                    case EntityState.Modified:
                        int oldRating = (int) entry.OriginalValues[nameof(entry.Entity.Rating)]!;
                        dbRatings.Remove(oldRating);
                        dbRatings.Add(entry.Entity.Rating);
                        break;
                    case EntityState.Deleted:
                        int deletedRating = (int) entry.OriginalValues[nameof(entry.Entity.Rating)]!;
                        dbRatings.Remove(deletedRating);
                        break;
                }

                product.TotalRating = dbRatings
                    .DefaultIfEmpty()
                    .Average();

                Entry(product).State = EntityState.Modified;
            }
        }
    }
}
