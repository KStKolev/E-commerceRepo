using E_commerceApplication.Business.Interfaces;
using E_commerceApplication.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public GamesController(IGamesService gamesService)
        {
            _gamesService = gamesService;
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
            var searchedData = await _gamesService
                .GetSearchedGamesAsync(term, limit, offset);

            return Ok(searchedData);
        }
    }
}