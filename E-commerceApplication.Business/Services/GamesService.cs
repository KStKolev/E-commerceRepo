using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.Business.Resources;
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

        public async Task<int> CreateGameAsync(GamesModel gameModel)
        {
            Product createdProduct = new()
            {
                Name = gameModel.Name,
                Genre = gameModel.Genre,
                Platform = gameModel.Platform,
                Logo = gameModel.Logo,
                Background = gameModel.Background,
                Rating = gameModel.Rating,
                DateCreated = DateTime.UtcNow,
                Count = gameModel.Count,
                Price = gameModel.Price
            };

            Product game = await _productRepository
                .CreateProductAsync(createdProduct);

            return game.Id;
        }

        public async Task DeleteGameAsync(int gameId)
        {
            Product? product = await _productRepository
                .GetProductByIdAsync(gameId);

            if (product == null)
            {
                throw new ArgumentException(ExceptionMessages.ProductNotFound);
            }

            await _productRepository
                .DeleteProductAsync(product);
        }

        public async Task<Product?> GetGameByIdAsync(int gameId)
        {
            return await _productRepository
                .GetProductByIdAsync(gameId);
        }

        public async Task<List<Product>> GetSearchedGamesAsync(string term, int limit, int offset)
        {
            return await _productRepository
                .SearchProductsAsync(term, limit, offset);
        }

        public async Task<List<Platforms>> GetTopGamePlatformsAsync()
        {
            return await _productRepository
                .GetTopProductPlatformsAsync();
        }

        public async Task UpdateGameAsync(UpdateGamesModel gameModel)
        {
            Product? product = await _productRepository
                .GetProductByIdAsync(gameModel.Id);

            if (product == null)
            {
                throw new ArgumentException(ExceptionMessages.ProductNotFound);
            }

            product.Name = gameModel.Name;
            product.Genre = gameModel.Genre;
            product.Platform = gameModel.Platform;
            product.Logo = gameModel.Logo;
            product.Background = gameModel.Background;
            product.Rating = gameModel.Rating;
            product.Count = gameModel.Count;
            product.Price = gameModel.Price;

            await _productRepository
                .UpdateProductAsync(product);
        }
    }
}