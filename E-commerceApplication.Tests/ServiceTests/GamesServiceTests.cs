using E_commerceApplication.Business.Services;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DAL.Interfaces;
using Moq;

namespace E_commerceApplication.Tests.ServiceTests
{
    public class GamesServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly GamesService _gamesService;

        public GamesServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _gamesService = new GamesService(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetSearchedGamesAsync_ShouldReturnFilteredProducts()
        {
            string searchTerm = "Ha";
            int limit = 10;
            int offset = 0;
            int resultCount = 2;

            string productName1 = "Halo Infinite";
            string productName2 = "Half-Life";
            string productName3 = "God of War";

            var products = new List<Product>
            {
                new Product { Id = 1, Name = productName1, Platform = Platforms.Mobile },
                new Product { Id = 2, Name = productName2, Platform = Platforms.Web },
                new Product { Id = 3, Name = productName3, Platform = Platforms.Console }
            };

            _productRepositoryMock
                .Setup(repo => repo.SearchProductsAsync(searchTerm, limit, offset))
                .ReturnsAsync(products.Where(p => p.Name.Contains(searchTerm)).ToList());

            var result = await _gamesService
                .GetSearchedGamesAsync(searchTerm, limit, offset);

            Assert.NotNull(result);
            Assert.Equal(resultCount, result.Count);
            Assert.Contains(result, p => p.Name == productName1);
            Assert.Contains(result, p => p.Name == productName2);
        }

        [Fact]
        public async Task GetSearchedGamesAsync_ShouldReturnEmptyList_WhenNoMatches()
        {
            string searchTerm = "Zelda";
            int limit = 10;
            int offset = 0;

            _productRepositoryMock
                .Setup(repo => repo.SearchProductsAsync(searchTerm, limit, offset))
                .ReturnsAsync(new List<Product>());

            var result = await _gamesService
                .GetSearchedGamesAsync(searchTerm, limit, offset);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetMostPopularPlatformsAsync_ShouldReturnTopPlatforms()
        {
            int topPlatformsAmount = 3;

            var platforms = new List<Platforms>
            {
                Platforms.Web,
                Platforms.Mobile,
                Platforms.Desktop
            };

            _productRepositoryMock
                .Setup(repo => repo.GetTopProductPlatformsAsync())
                .ReturnsAsync(platforms);

            var result = await _gamesService
                .GetTopGamePlatformsAsync();

            Assert.NotNull(result);
            Assert.Equal(topPlatformsAmount, result.Count);
            Assert.Equal(Platforms.Web, result[0]);
            Assert.Equal(Platforms.Mobile, result[1]);
            Assert.Equal(Platforms.Desktop, result[2]);
        }

        [Fact]
        public async Task GetMostPopularPlatformsAsync_ShouldReturnEmptyList_WhenNoPlatforms()
        {
            _productRepositoryMock
                .Setup(repo => repo.GetTopProductPlatformsAsync())
                .ReturnsAsync(new List<Platforms>());

            var result = await _gamesService
                .GetTopGamePlatformsAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
