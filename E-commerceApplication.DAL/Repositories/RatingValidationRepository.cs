using E_commerceApplication.DAL.Data;
using E_commerceApplication.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerceApplication.DAL.Repositories
{
    public class RatingValidationRepository : IRatingValidationRepository
    {
        private readonly ApplicationDbContext _context;

        public RatingValidationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckProductByIdAsync(int productId)
        {
            return await _context
                .Products
                .AnyAsync(p => p.Id == productId);
        }
    }
}
