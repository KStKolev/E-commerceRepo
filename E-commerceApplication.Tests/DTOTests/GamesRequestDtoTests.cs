using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DTOs;
using Microsoft.AspNetCore.Http;
using Moq;

namespace E_commerceApplication.Tests.DTOTests
{
    public class GamesRequestDtoTests
    {
        [Fact]
        public void GameDto_MissingRequiredFields_FailsValidation()
        {
            var dto = new GamesDto();

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(dto.Name)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(dto.Genre)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(dto.Logo)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(dto.Background)));
        }

        [Fact]
        public void GameDto_InvalidValueTypes_ShouldReturnValidationErrors()
        {
            var fileMock = new Mock<IFormFile>()
                .Object;

            string name = "Test Game";
            string genre = "Action";
            int invalidPlatformValue = 1000;
            int invalidRatingValue = 1000;
            int invalidCountValue = -1;
            decimal invalidPriceValue = -1m;

            var dto = new GamesDto
            {
                Name = name,
                Genre = genre,
                Platform = (Platforms)invalidPlatformValue,
                Logo = fileMock,
                Background = fileMock,
                Rating = (Rating)invalidRatingValue,
                Count = invalidCountValue,
                Price = invalidPriceValue
            };

            var results = ValidationHelper.ValidateModel(dto);

            Assert.Contains(results, r => r.ErrorMessage!.Contains(nameof(dto.Rating)));
            Assert.Contains(results, r => r.ErrorMessage!.Contains(nameof(dto.Platform)));
            Assert.Contains(results, r => r.ErrorMessage!.Contains(nameof(dto.Count)));
            Assert.Contains(results, r => r.ErrorMessage!.Contains(nameof(dto.Price)));
        }

        [Fact]
        public void CreateGameDto_AllValidFields_PassesValidation()
        {
            var fileMock = new Mock<IFormFile>()
                .Object;
            string gameName = "Test Game";
            string genre = "Action";
            int count = 10;
            decimal price = 49.99m;

            var dto = new GamesDto
            {
                Name = gameName,
                Genre = genre,
                Platform = Platforms.Desktop,
                Logo = fileMock,
                Background = fileMock,
                Rating = Rating.All,
                Count = count,
                Price = price
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Empty(results);
        }

        [Fact]
        public void UpdateGameDto_AllValidFields_PassesValidation()
        {
            var fileMock = new Mock<IFormFile>()
                .Object;

            int gameId = 1;
            string gameName = "Updated Game";
            string genre = "Adventure";
            int count = 20;
            decimal price = 59.99m;

            var dto = new UpdateGamesDto
            {
                Id = gameId,
                Name = gameName,
                Genre = genre,
                Platform = Platforms.Web,
                Logo = fileMock,
                Background = fileMock,
                Rating = Rating.Six,
                Count = count,
                Price = price
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert.Empty(results);
        }
    }
}
