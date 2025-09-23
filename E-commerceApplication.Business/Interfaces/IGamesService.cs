using E_commerceApplication.Business.Models;
using E_commerceApplication.DAL.Entities;

namespace E_commerceApplication.Business.Interfaces
{
    public interface IGamesService
    {
        Task<List<Platforms>> GetTopGamePlatformsAsync();

        Task<List<Product>> GetSearchedGamesAsync(string term, int limit, int offset);

        Task<Product?> GetGameByIdAsync(int gameId);

        Task<int> CreateGameAsync(GamesModel gameModel);

        Task<bool> UpdateGameAsync(UpdateGamesModel gameModel);

        Task<bool> DeleteGameAsync(int gameId);

        Task<PaginatedResponseModel<Product>> GetPaginatedGamesAsync(GameFilterAndSortModel gameListModel, 
            PaginationRequestModel paginationRequestModel);
    }
}