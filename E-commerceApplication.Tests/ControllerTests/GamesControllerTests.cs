using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.Controllers;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DTOs;
using E_commerceApplication.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Moq;

namespace E_commerceApplication.Tests.ControllerTests
{
    public class GamesControllerTests
    {
        private readonly Mock<IGamesService> _mockGamesService;
        private readonly Mock<IImageService> _mockImageService;
        private readonly Mock<IRatingService> _mockRatingService;
        private readonly GamesController _controller;

        public GamesControllerTests()
        {
            _mockGamesService = new Mock<IGamesService>();
            _mockImageService = new Mock<IImageService>();
            _mockRatingService = new Mock<IRatingService>();

            _controller = new GamesController(_mockGamesService.Object, 
                _mockImageService.Object, _mockRatingService.Object);
        }

        [Fact]
        public async Task GetTopPlatforms_ReturnsOkResult_WithListOfPlatforms()
        {
            var platforms = new List<Platforms>() { Platforms.Web, Platforms.Desktop, Platforms.Console };
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

            var products = new List<Product>()
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
            var product = new Product() { Id = gameId, Name = gameName };

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

            var notFoundResult = Assert
                .IsType<NotFoundObjectResult>(result);

            Assert
                .Equal(GamesControllerFailedActionsMessages.NotFoundGameRequestMessage,
                notFoundResult.Value);
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
                Rating = Rating.Twelve,
                Count = count,
                Price = price
            };

            _mockImageService
                .Setup(s => s
                .UploadImageAsync(gameDto.Logo))
                .ReturnsAsync(logoUrl);

            _mockImageService
                .Setup(s => s
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
                Rating = Rating.Eighteen,
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
                    .ReturnsAsync(true);

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
        public async Task UpdateGame_NonExistentGame_ReturnsBadRequest()
        {
            int gameId = 99;
            string gameName = "Missing Game";

            var dto = new UpdateGamesDto
            {
                Id = gameId,
                Name = gameName
            };

            _mockGamesService
                .Setup(s => s.UpdateGameAsync(It.IsAny<UpdateGamesModel>()))
                    .ReturnsAsync(false);

            var result = await _controller
                .UpdateGame(dto);

            var badRequestResult = Assert
                .IsType<BadRequestObjectResult>(result);

            Assert
                .Equal(GamesControllerFailedActionsMessages.UpdateGameBadRequestMessage,
                badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteGame_GameExists_ReturnsNoContent()
        {
            int gameId = 1;
            string gameName = "Test Game";
            var product = new Product() { Id = gameId, Name = gameName };

            _mockGamesService
                .Setup(s => s.GetGameByIdAsync(gameId))
                    .ReturnsAsync(product);

            _mockGamesService
                .Setup(s => s.DeleteGameAsync(gameId))
                    .ReturnsAsync(true);

            var result = await _controller
                .DeleteGame(gameId);

            Assert
                .IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteGame_GameDoesNotExist_ReturnsBadRequest()
        {
            int gameId = 1;

            _mockGamesService
                .Setup(s => s.DeleteGameAsync(gameId))
                    .ReturnsAsync(false);

            var result = await _controller
                .DeleteGame(gameId);

            var badRequestResult = Assert
                .IsType<BadRequestObjectResult>(result);

            Assert
                .Equal(GamesControllerFailedActionsMessages.DeleteGameBadRequestMessage,
                badRequestResult.Value);
        }

        [Fact]
        public async Task EditRating_ReturnsOk_WhenEditedSuccessfully()
        {
            var userId = Guid
                .NewGuid();

            ControllerTestHelper
                .SetUser(_controller, userId);

            int productId = 1;
            int ratingValue = 5;

            var dto = new EditRatingRequestDto
            {
                ProductId = productId,
                Rating = ratingValue
            };

            _mockRatingService
                .Setup(s => s.EditRatingGameAsync(It.IsAny<EditRatingModel>()))
                    .ReturnsAsync(true);

            var result = await _controller
                .EditRating(dto);

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var model = Assert
                .IsType<EditRatingModel>(okResult.Value);

            Assert
                .Equal(dto.ProductId, model.ProductId);

            Assert
                .Equal(userId, model.UserId);

            Assert
                .Equal(dto.Rating, model.Rating);
        }

        [Fact]
        public async Task EditRating_ReturnsBadRequest_WhenEditFails()
        {
            var userId = Guid
                .NewGuid();

            ControllerTestHelper
                .SetUser(_controller, userId);

            int productId = 1;
            int ratingValue = 5;

            var dto = new EditRatingRequestDto
            {
                ProductId = productId,
                Rating = ratingValue
            };

            _mockRatingService
                .Setup(s => s.EditRatingGameAsync(It.IsAny<EditRatingModel>()))
                    .ReturnsAsync(false);

            var result = await _controller
                .EditRating(dto);

            var badRequestResult = Assert
                .IsType<BadRequestObjectResult>(result);

            Assert
                .Equal(GamesControllerFailedActionsMessages.EditRatingBadRequestMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteRating_ReturnsNoContent_WhenDeletedSuccessfully()
        {
            var userId = Guid
                .NewGuid();

            ControllerTestHelper
                .SetUser(_controller, userId);

            int productId1 = 1;
            int productId2 = 2;

            var dto = new DeleteRatingRequestDto
            {
                ProductIds = new List<int> { productId1, productId2 }
            };

            _mockRatingService
                .Setup(s => s.DeleteRatingsAsync(It.IsAny<DeleteRatingModel>()))
                    .Returns(Task.CompletedTask);

            var result = await _controller
                .DeleteRating(dto);

            Assert
                .IsType<NoContentResult>(result);
        }
        
        [Fact]
        public async Task GetGameList_ReturnsOk_WithPaginatedGames()
        {
            int productId = 1;
            string productName = "Game 1";
            int page = 1;
            int pageSize = 10;
            string genre = "Action";

            var paginatedGames = new PaginatedResponseModel<Product>
            {
                Items = new List<Product> { new Product { Id = productId, Name = productName } },
                Page = page,
                PageSize = pageSize
            };

            _mockGamesService
                .Setup(s => s.GetPaginatedGamesAsync(It.IsAny<GameFilterAndSortModel>(), It.IsAny<PaginationRequestModel>()))
                    .ReturnsAsync(paginatedGames);

            var filterDto = new GameFilterAndSortRequestDto
            {
                Genres = new List<string> { genre },
                Age = Rating.Six,
                SortBy = SortByField.Price,
                SortOrder = SortOrder.Asc
            };

            var paginationDto = new PaginationRequestDto { Page = page, PageSize = pageSize };

            var result = await _controller
                .GetGameList(filterDto, paginationDto);

            var okResult = Assert
                .IsType<OkObjectResult>(result);

            var model = Assert
                .IsType<PaginatedResponseModel<Product>>(okResult.Value);

            Assert
                .Single(model.Items);

            Assert
                .Equal(page, model.Page);

            Assert
                .Equal(pageSize, model.PageSize);
        }
    }
}