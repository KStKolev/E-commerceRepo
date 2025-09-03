using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;

namespace E_commerceApplication.Business.Services
{
    public class GamesService : IGamesService
    {
        private readonly IProductRepository _productRepository;

        public GamesService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetSearchedGamesAsync(string term, int limit, int offset)
        {
            return await _productRepository.SearchProductsAsync(term, limit, offset);
        }

        public async Task<List<Platforms>> GetTopGamePlatformsAsync()
        {
            return await _productRepository.GetTopProductPlatformsAsync();
        }
    }
}
