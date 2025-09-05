using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.Controllers;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DTOs;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace E_commerceApplication.Tests.ControllerTests
{
    public class GamesControllerTests
    {
        private readonly Mock<IGamesService> _mockGamesService;
        private readonly Mock<IImageService> _mockImageService;
        private readonly GamesController _controller;

        public GamesControllerTests()
        {
            _mockGamesService = new Mock<IGamesService>();
            _mockImageService = new Mock<IImageService>();
            _controller = new GamesController(_mockGamesService.Object, _mockImageService.Object);
        }

        [Fact]
        public async Task GetTopPlatforms_ReturnsOkResult_WithListOfPlatforms()
        {
            List<Platforms> platforms = new() { Platforms.Web, Platforms.Desktop, Platforms.Console };
            int takeCount = 3;

            _mockGamesService
                .Setup(service => service.GetTopGamePlatformsAsync())
                .ReturnsAsync(platforms);

            var result = await _controller
                .GetTopPlatforms();

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var returnValue = Assert
                .IsType<List<Platforms>>(okResult.Value);

            Assert
                .Equal(takeCount, returnValue.Count);
        }

        [Fact]
        public async Task GetSearchedData_ReturnsOkResult_WithListOfGames()
        {
            string term = "game";
            int limit = 5;
            int offset = 0;

            List<Product> products = new()
            {
                new Product { Id = 1, Name = "Game 1" },
                new Product { Id = 2, Name = "Game 2" }
            };

            _mockGamesService
                .Setup(service => service
                .GetSearchedGamesAsync(term, limit, offset))
                .ReturnsAsync(products);

            var result = await _controller
                .GetSearchedData(term, limit, offset);

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var returnValue = Assert
                .IsType<List<Product>>(okResult.Value);

            Assert
                .Equal(products.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetGameById_GameExists_ReturnsOk()
        {
            int gameId = 1;
            string gameName = "Game 1";
            Product product = new() { Id = gameId, Name = gameName };

            _mockGamesService
                .Setup(s => s.GetGameByIdAsync(gameId))
                .ReturnsAsync(product);

            var result = await _controller
                .GetGameById(gameId);

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            Assert
                .Equal(product, okResult.Value);
        }

        [Fact]
        public async Task GetGameById_GameDoesNotExist_ReturnsNotFound()
        {
            int gameId = 1;

            _mockGamesService
                .Setup(s => s.GetGameByIdAsync(gameId))
                .ReturnsAsync((Product?)null);

            var result = await _controller
                .GetGameById(gameId);

            Assert
                .IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateGame_ValidDto_ReturnsCreatedAtAction()
        {
            var logoFileMock = new Mock<IFormFile>();
            var backgroundFileMock = new Mock<IFormFile>();

            string logoFileName = "logo.png";
            string backgroundFileName = "background.png";
            string logoUrl = "logoUrl";
            string backgroundUrl = "backgroundUrl";

            int gameId = 1;
            string gameName = "New Game";
            string genre = "RPG";
            int count = 5;
            decimal price = 49.99m;

            logoFileMock
                .Setup(f => f.FileName)
                .Returns(logoFileName);

            logoFileMock
                .Setup(f => f.OpenReadStream())
                .Returns(new MemoryStream());

            backgroundFileMock
                .Setup(f => f.FileName)
                .Returns(backgroundFileName);

            backgroundFileMock
                .Setup(f => f.OpenReadStream())
                .Returns(new MemoryStream());

            var gameDto = new GamesDto
            {
                Name = gameName,
                Genre = genre,
                Platform = Platforms.Web,
                Logo = logoFileMock.Object,
                Background = backgroundFileMock.Object,
                Rating = Rating.Teen,
                Count = count,
                Price = price
            };

            _mockImageService.Setup(s => s
                .UploadImageAsync(gameDto.Logo))
                .ReturnsAsync(logoUrl);

            _mockImageService.Setup(s => s
                .UploadImageAsync(gameDto.Background))
                .ReturnsAsync(backgroundUrl);

            _mockGamesService
                .Setup(s => s.CreateGameAsync(It.IsAny<GamesModel>()))
                .ReturnsAsync(gameId);

            var result = await _controller
                .CreateGame(gameDto);

            var createdResult = Assert
                .IsType<CreatedAtActionResult>(result);

            var returnedModel = Assert
                .IsType<GamesModel>(createdResult.Value);

            Assert
                .Equal(logoUrl, returnedModel.Logo);

            Assert
                .Equal(backgroundUrl, returnedModel.Background);
        }

        [Fact]
        public async Task UpdateGame_ValidDto_ReturnsOk()
        {
            var logoFileMock = new Mock<IFormFile>();
            var backgroundFileMock = new Mock<IFormFile>();

            string logoName = "logo.png";
            string backgroundName = "background.png";
            string backgroundUrl = "backgroundUrl";
            string logoUrl = "logoUrl";

            string gameName = "Updated Game";
            string genre = "Action";
            int count = 10;
            decimal price = 59.99m;

            logoFileMock
                .Setup(f => f.FileName)
                .Returns(logoName);

            logoFileMock
                .Setup(f => f.OpenReadStream())
                .Returns(new MemoryStream());

            backgroundFileMock
                .Setup(f => f.FileName)
                .Returns(backgroundName);

            backgroundFileMock
                .Setup(f => f.OpenReadStream())
                .Returns(new MemoryStream());

            var updateDto = new UpdateGamesDto
            {
                Id = 1,
                Name = gameName,
                Genre = genre,
                Platform = Platforms.Web,
                Logo = logoFileMock.Object,
                Background = backgroundFileMock.Object,
                Rating = Rating.Mature,
                Count = count,
                Price = price
            };

            _mockImageService
                .Setup(s => s.UploadImageAsync(updateDto.Logo))
                .ReturnsAsync(logoUrl);

            _mockImageService
                .Setup(s => s.UploadImageAsync(updateDto.Background))
                .ReturnsAsync(backgroundUrl);

            _mockGamesService
                .Setup(s => s.UpdateGameAsync(It.IsAny<UpdateGamesModel>()))
                .Returns(Task.CompletedTask);

            var result = await _controller
                .UpdateGame(updateDto);

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var returnedModel = Assert
                .IsType<UpdateGamesModel>(okResult.Value);

            Assert
                .Equal(logoUrl, returnedModel.Logo);

            Assert
                .Equal(backgroundUrl, returnedModel.Background);
        }

        [Fact]
        public async Task UpdateGame_NonExistentGame_ReturnsNotFound()
        {
            int gameId = 99;
            string gameName = "Missing Game";
            string notFoundMessage = "Product not found";

            var dto = new UpdateGamesDto
            {
                Id = gameId,
                Name = gameName
            };

            _mockGamesService
                .Setup(s => s.UpdateGameAsync(It.IsAny<UpdateGamesModel>()))
                .ThrowsAsync(new ArgumentException(notFoundMessage));

            var result = await _controller
                .UpdateGame(dto);

            Assert
                .IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteGame_GameExists_ReturnsNoContent()
        {
            int gameId = 1;
            string gameName = "Test Game";
            Product product = new() { Id = gameId, Name = gameName };

            _mockGamesService
                .Setup(s => s.GetGameByIdAsync(gameId))
                .ReturnsAsync(product);

            _mockGamesService
                .Setup(s => s.DeleteGameAsync(gameId))
                .Returns(Task.CompletedTask);

            var result = await _controller
                .DeleteGame(gameId);

            Assert
                .IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteGame_GameDoesNotExist_ReturnsNotFound()
        {
            int gameId = 1;
            string exceptionMessage = "Product not found";

            _mockGamesService
                .Setup(s => s.DeleteGameAsync(gameId))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            var result = await _controller
                .DeleteGame(gameId);

            Assert
                .IsType<NotFoundObjectResult>(result);
        }
    }
}