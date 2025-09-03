using E_commerceApplication.DAL.Data;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerceApplication.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> SearchProductsAsync(string term, int limit, int offset)
        {
            var query = _context
                .Products
                .AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query
                    .Where(p => p.Name.Contains(term));
            }

            return await query
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<Platforms>> GetTopProductPlatformsAsync()
        {
            int takeCount = 3;

            return await _context
                .Products
                .GroupBy(p => p.Platform)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(takeCount)
                .ToListAsync();
        }
    }
}
