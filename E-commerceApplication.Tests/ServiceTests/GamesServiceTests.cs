using E_commerceApplication.Business.Models;
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

        [Fact]
        public async Task GetGameByIdAsync_ShouldReturnGame_WhenGameExists()
        {
            int gameId = 1;
            string productName = "Halo Infinite";

            var product = new Product
            {
                Id = gameId,
                Name = productName,
                Platform = Platforms.Console
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByIdAsync(gameId))
                .ReturnsAsync(product);

            var result = await _gamesService
                .GetGameByIdAsync(gameId);

            Assert
                .NotNull(result);

            Assert
                .Equal(gameId, result!.Id);
        }

        [Fact]
        public async Task GetGameByIdAsync_ShouldReturnNull_WhenGameDoesNotExist()
        {
            int gameId = 1;

            _productRepositoryMock
                .Setup(repo => repo.GetProductByIdAsync(gameId))
                .ReturnsAsync((Product?)null);

            var result = await _gamesService
                .GetGameByIdAsync(gameId);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateGameAsync_ShouldReturnNewGameId()
        {
            var gameId = 1;
            string gameName = "Halo Infinite";
            string genre = "Action";
            string logoName = "logo.png";
            string backgroundName = "background.png";
            int count = 100;
            decimal price = 59.99m;

            var gameModel = new GamesModel
            {
                Name = gameName,
                Genre = genre,
                Platform = Platforms.Console,
                Logo = logoName,
                Background = backgroundName,
                Rating = Rating.Eighteen,
                Count = count,
                Price = price
            };

            var createdProduct = new Product
            {
                Id = gameId,
                Name = gameModel.Name,
                Genre = gameModel.Genre,
                Platform = gameModel.Platform,
                Logo = gameModel.Logo,
                Background = gameModel.Background,
                Rating = gameModel.Rating,
                Count = gameModel.Count,
                Price = gameModel.Price,
                DateCreated = DateTime.UtcNow
            };

            _productRepositoryMock
                .Setup(repo => repo.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            var result = await _gamesService
                .CreateGameAsync(gameModel);

            Assert
                .Equal(createdProduct.Id, result);
        }

        [Fact]
        public async Task UpdateGameAsync_ShouldUpdateGame_WhenGameExists()
        {
            int gameId = 1;
            string gameName = "Halo Infinite";
            string gameNameUpdated = "Halo Infinite Updated";
            string genre = "Action";
            string logoName = "logo_updated.png";
            string backgroundName = "background_updated.png";
            int count = 150;
            decimal price = 49.99m;

            var existingProduct = new Product
            {
                Id = gameId,
                Name = gameName,
                Platform = Platforms.Console
            };

            var updatedGameModel = new UpdateGamesModel
            {
                Id = gameId,
                Name = gameNameUpdated,
                Genre = genre,
                Platform = Platforms.Console,
                Logo = logoName,
                Background = backgroundName,
                Rating = Rating.Eighteen,
                Count = count,
                Price = price
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByIdAsync(gameId))
                .ReturnsAsync(existingProduct);

            _productRepositoryMock
                .Setup(repo => repo.UpdateProductAsync(existingProduct))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _gamesService
                .UpdateGameAsync(updatedGameModel);

            _productRepositoryMock
                .Verify(repo => repo.UpdateProductAsync(existingProduct), Times.Once);

            Assert
                .Equal(updatedGameModel.Name, existingProduct.Name);

            Assert
                .Equal(updatedGameModel.Genre, existingProduct.Genre);

            Assert
                .Equal(updatedGameModel.Platform, existingProduct.Platform);

            Assert
                .Equal(updatedGameModel.Logo, existingProduct.Logo);

            Assert
                .Equal(updatedGameModel.Background, existingProduct.Background);

            Assert
                .Equal(updatedGameModel.Rating, existingProduct.Rating);

            Assert
                .Equal(updatedGameModel.Count, existingProduct.Count);

            Assert
                .Equal(updatedGameModel.Price, existingProduct.Price);
        }

        [Fact]
        public async Task UpdateGameAsync_ShouldThrowArgumentException_WhenGameDoesNotExist()
        {
            int gameId = 1;
            string gameName = "Halo Infinite updated";
            string genre = "Action";
            string logoName = "logo_updated.png";
            string backgroundName = "background_updated.png";
            int count = 150;
            decimal price = 49.99m;
            string productNotFoundMessage = "Product not found";

            var updatedGameModel = new UpdateGamesModel
            {
                Id = gameId,
                Name = gameName,
                Genre = genre,
                Platform = Platforms.Console,
                Logo = logoName,
                Background = backgroundName,
                Rating = Rating.Eighteen,
                Count = count,
                Price = price
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByIdAsync(gameId))
                .ReturnsAsync((Product?)null);

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await _gamesService.UpdateGameAsync(updatedGameModel));

            Assert
                .Equal(productNotFoundMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteGameAsync_ShouldDeleteGame_WhenGameExists()
        {
            int gameId = 1;
            string gameName = "Halo Infinite";

            var product = new Product
            {
                Id = gameId,
                Name = gameName,
                Platform = Platforms.Console
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByIdAsync(gameId))
                .ReturnsAsync(product);

            _productRepositoryMock
                .Setup(repo => repo.DeleteProductAsync(product))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _gamesService
                .DeleteGameAsync(gameId);

            _productRepositoryMock
                .Verify(repo => repo.DeleteProductAsync(product), Times.Once);
        }

        [Fact]
        public async Task DeleteGameAsync_ShouldThrowArgumentException_WhenGameDoesNotExist()
        {
            int gameId = 1;
            string productNotFoundMessage = "Product not found";

            _productRepositoryMock
                .Setup(repo => repo.GetProductByIdAsync(gameId))
                .ReturnsAsync((Product?)null);

            var exception = await Assert
                .ThrowsAsync<ArgumentException>(async () => await _gamesService.DeleteGameAsync(gameId));

            Assert
                .Equal(productNotFoundMessage, exception.Message);
        }
    }
}