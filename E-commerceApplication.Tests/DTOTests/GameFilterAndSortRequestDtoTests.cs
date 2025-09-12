using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DTOs;

namespace E_commerceApplication.Tests.DTOTests
{
    public class GameFilterAndSortRequestDtoTests
    {
        [Fact]
        public void ValidDto_ShouldPassValidation()
        {
            string genreAction = "Action";
            string genreRpg = "RPG";

            var dto = new GameFilterAndSortRequestDto
            {
                Genres = new List<string> { genreAction, genreRpg },
                Age = Rating.Twelve,
                SortBy = SortByField.Price,
                SortOrder = SortOrder.Desc
            };

            var results = ValidationHelper
                .ValidateModel(dto);

            Assert
                .Empty(results);
        }

        [Fact]
        public void Genres_WithEmptyString_ShouldFailValidation()
        {
            string genreAction = "Action";
            string genreEmpty = "";

            var dto = new GameFilterAndSortRequestDto
            {
                Genres = new List<string> { genreAction, genreEmpty },
                Age = Rating.Six,
                SortBy = SortByField.Price,
                SortOrder = SortOrder.Asc
            };

            Assert
                .Contains(dto.Genres, string.IsNullOrWhiteSpace);
        }

        [Fact]
        public void Genres_EmptyList_ShouldFailValidation()
        {
            var dto = new GameFilterAndSortRequestDto
            {
                Genres = new List<string>(),
                Age = Rating.Six,
                SortBy = SortByField.Price,
                SortOrder = SortOrder.Asc
            };

            Assert
               .Empty(dto.Genres);
        }

        [Fact]
        public void InvalidAge_ShouldBeDetected()
        {
            string genreShooter = "Shooter";
            int invalidAge = -1;

            var dto = new GameFilterAndSortRequestDto
            {
                Genres = new List<string> { genreShooter },
                Age = (Rating) invalidAge,
                SortBy = SortByField.Rating,
                SortOrder = SortOrder.Asc
            };

            bool valid = Enum
                .IsDefined(typeof(Rating), dto.Age);

            Assert
                .False(valid);
        }

        [Fact]
        public void InvalidSortBy_ShouldBeDetected()
        {
            string genreStrategy = "Strategy";
            int invalidSortBy = -1;

            var dto = new GameFilterAndSortRequestDto
            {
                Genres = new List<string> { genreStrategy },
                Age = Rating.Eighteen,
                SortBy = (SortByField) invalidSortBy,
                SortOrder = SortOrder.Desc
            };

            bool valid = Enum
                .IsDefined(typeof(SortByField), dto.SortBy);

            Assert
                .False(valid);
        }

        [Fact]
        public void InvalidSortOrder_ShouldBeDetected()
        {
            string genreAdventure = "Adventure";
            int invalidSortOrder = -1;

            var dto = new GameFilterAndSortRequestDto
            {
                Genres = new List<string> { genreAdventure },
                Age = Rating.All,
                SortBy = SortByField.Price,
                SortOrder = (SortOrder) invalidSortOrder
            };

            bool valid = Enum
                .IsDefined(typeof(SortOrder), dto.SortOrder);

            Assert
                .False(valid);
        }
    }
}