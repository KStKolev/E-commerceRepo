using E_commerceApplication.Business;
using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.Business.Models;
using E_commerceApplication.DAL.Entities;
using E_commerceApplication.DTOs;
using E_commerceApplication.Resources;
using E_commerceApplication.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerceApplication.Controllers
{
    /// <summary>
    /// Controller for handling fetching data related to product items.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _gamesService;
        private readonly IImageService _imageService;
        private readonly IRatingService _ratingService;

        public GamesController(IGamesService gamesService, 
            IImageService imageService, IRatingService ratingService)
        {
            _gamesService = gamesService;
            _imageService = imageService;
            _ratingService = ratingService;
        }

        /// <summary>
        /// Fetches a list of the tree most popular platforms. 
        /// </summary>
        /// <returns>
        /// A list of the three most popular platforms. 
        /// </returns>
        [AllowAnonymous]
        [HttpGet("topPlatforms")]
        public async Task<IActionResult> GetTopPlatforms()
        {
            List<Platforms> topPlatforms = await _gamesService
                .GetTopGamePlatformsAsync();

            return Ok(topPlatforms);
        }

        /// <summary>
        /// Fetches a list of searched data based on the provided term, limit, and offset. 
        /// </summary>
        /// <param name="term">
        /// The search term to filter the data. 
        /// </param>
        /// <param name="limit">
        /// The maximum number of results to return. 
        /// </param>
        /// <param name="offset">
        /// The number of results to skip before starting to collect the result set. 
        /// </param>
        /// <returns>
        /// A list of searched data matching the provided term, limited by the specified limit and offset. 
        /// </returns>
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> GetSearchedData([FromQuery] string term,
            [FromQuery] int limit, [FromQuery] int offset)
        {
            List<Product> searchedData = await _gamesService
                .GetSearchedGamesAsync(term, limit, offset);

            return Ok(searchedData);
        }

        /// <summary>
        /// Fetches a game by its unique identifier. 
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the game to fetch. 
        /// </param>
        /// <returns>
        /// The game matching the provided identifier, or a NotFound result if no such game exists. 
        /// </returns>
        [AllowAnonymous]
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            Product? product = await _gamesService
                .GetGameByIdAsync(id);

            if (product == null)
            {
                return NotFound(string.Format(ControllerExceptionMessages.ProductNotFound, id));
            }

            return Ok(product);
        }

        /// <summary>
        /// Creates a new game with the provided details. 
        /// </summary>
        /// <param name="gameDto">
        /// The details of the game to create, including its name, genre, platform, logo, background, rating, count, and price. 
        /// </param>
        /// <returns>
        /// A CreatedAtAction result with the details of the newly created game. 
        /// </returns>
        [Authorize(Roles = nameof(RoleType.Admin))]
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromForm] GamesDto gameDto)
        {
            string logoUrl = gameDto.Logo != null ?
                await _imageService.UploadImageAsync(gameDto.Logo)
                : string.Empty;

            string backgroundUrl = gameDto.Background != null ?
                await _imageService.UploadImageAsync(gameDto.Background)
                : string.Empty;

            GamesModel gameModel = new GamesModel
            {
                Name = gameDto.Name,
                Genre = gameDto.Genre,
                Platform = gameDto.Platform,
                Logo = logoUrl,
                Background = backgroundUrl,
                Rating = gameDto.Rating,
                Count = gameDto.Count,
                Price = gameDto.Price
            };

            int gameId = await _gamesService
                .CreateGameAsync(gameModel);

            return CreatedAtAction(nameof(GetGameById), new { id = gameId }, gameModel);
        }

        /// <summary>
        /// Updates an existing game with the provided details.
        /// </summary>
        /// <param name="updateGameModelDto">
        /// The details of the game to update, including its unique identifier and new values for its properties. 
        /// </param>
        /// <returns>
        /// An Ok result with the updated game details if the update is successful. 
        /// </returns>
        [Authorize(Roles = nameof(RoleType.Admin))]
        [HttpPut]
        public async Task<IActionResult> UpdateGame([FromForm] UpdateGamesDto updateGameModelDto)
        {
            string logoUrl = updateGameModelDto.Logo != null ? 
                await _imageService.UploadImageAsync(updateGameModelDto.Logo) 
                : string.Empty;

            string backgroundUrl = updateGameModelDto.Background != null ? 
                await _imageService.UploadImageAsync(updateGameModelDto.Background) 
                : string.Empty;

            UpdateGamesModel updateGameModel = new UpdateGamesModel
            {
                Id = updateGameModelDto.Id,
                Name = updateGameModelDto.Name,
                Genre = updateGameModelDto.Genre,
                Platform = updateGameModelDto.Platform,
                Logo = logoUrl,
                Background = backgroundUrl,
                Rating = updateGameModelDto.Rating,
                Count = updateGameModelDto.Count,
                Price = updateGameModelDto.Price
            };

            try
            {
                await _gamesService
                    .UpdateGameAsync(updateGameModel);

                return Ok(updateGameModel);
            }
            catch (ArgumentException exception)
            {
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Deletes a game by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the game to delete. 
        /// </param>
        /// <returns>
        /// A NoContent result if the deletion is successful, or a NotFound result if no such game exists. 
        /// </returns>
        [Authorize(Roles = nameof(RoleType.Admin))]
        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            try
            {
                await _gamesService
                    .DeleteGameAsync(id);

                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound(string.Format(ControllerExceptionMessages.ProductNotFound, id));
            }
        }

        /// <summary>
        /// Edits the rating of a game by a user.
        /// </summary>
        /// <param name="editRatingRequestDto">
        /// The details of the rating to edit, including the product ID and the new rating value. 
        /// </param>
        /// <returns>
        /// An Ok result with the edited rating details if the edit is successful, or a BadRequest result if the edit fails. 
        /// </returns>
        [Authorize]
        [HttpPost("rating")]
        public async Task<IActionResult> EditRating([FromBody] EditRatingRequestDto editRatingRequestDto) 
        {
            string? userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            Guid.TryParse(userId, out Guid result);

            EditRatingModel editRatingModel = new EditRatingModel
            {
                ProductId = editRatingRequestDto.ProductId,
                UserId = result,
                Rating = editRatingRequestDto.Rating
            };

            bool isEdited = await _ratingService
                .EditRatingGameAsync(editRatingModel);

            if (!isEdited) 
            {
                return BadRequest(GamesControllerFailedActionsMessages.EditRatingBadRequestMessage);
            }

            return Ok(editRatingModel);
        }

        /// <summary>
        /// Deletes ratings for multiple products by a user.
        /// </summary>
        /// <param name="deleteRatingRequestDto">
        /// The details of the ratings to delete, including a list of product IDs. 
        /// </param>
        /// <returns>
        /// A NoContent result if the deletion is successful, or a BadRequest result if the deletion fails. 
        /// </returns>
        [Authorize]
        [HttpDelete("rating")]
        public async Task<IActionResult> DeleteRating([FromBody] DeleteRatingRequestDto deleteRatingRequestDto)
        {
            string? userId = User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            Guid.TryParse(userId, out Guid result);

            DeleteRatingModel deleteRatingModel = new DeleteRatingModel
            {
                UserId = result,
                ProductIds = deleteRatingRequestDto.ProductIds
            };

            bool areDeleted = await _ratingService
                .DeleteRatingsAsync(deleteRatingModel);

            if (!areDeleted)
            {
                return BadRequest(GamesControllerFailedActionsMessages.DeleteRatingsBadRequestMessage);
            }

            return NoContent();
        }

        /// <summary>
        /// Fetches a paginated list of games based on the provided filtering, sorting, and pagination parameters.
        /// </summary>
        /// <param name="gameFilterAndSortRequestDto">
        /// The filtering and sorting parameters for the game list. 
        /// </param>
        /// <param name="paginationRequestDto">
        /// The pagination parameters for the game list. 
        /// </param>
        /// <returns>
        /// A paginated list of games matching the provided filtering, sorting, and pagination parameters. 
        /// </returns>
        [AllowAnonymous]
        [HttpGet("list")]
        [ValidateGameListParams]
        public async Task<IActionResult> GetGameList([FromQuery] GameFilterAndSortRequestDto gameFilterAndSortRequestDto,
            [FromQuery] PaginationRequestDto paginationRequestDto) 
        {
            GameFilterAndSortModel gameFilterAndSortModel = new GameFilterAndSortModel
            {
                Genres = gameFilterAndSortRequestDto.Genres,
                Age = gameFilterAndSortRequestDto
                    .Age
                    .GetValueOrDefault(),
                SortBy = gameFilterAndSortRequestDto
                    .SortBy
                    .GetValueOrDefault(),
                SortOrder = gameFilterAndSortRequestDto
                    .SortOrder
                    .GetValueOrDefault()
            };

            PaginationRequestModel paginationRequestModel = new PaginationRequestModel
            {
                Page = paginationRequestDto.Page,
                PageSize = paginationRequestDto.PageSize
            };

            PaginatedResponseModel<Product> paginatedGames = await _gamesService
                .GetPaginatedGames(gameFilterAndSortModel, paginationRequestModel);

            return Ok(paginatedGames);
        }
    }
}