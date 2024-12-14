using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface ISaveMediaItemUsers
{
	// Watch History
	Task<BaseResult<List<MediaItemDto>>> GetWatchHistoryAsync(Guid userId, int pageNumber = 1, int pageSize = 10);

	Task<BaseResult<bool>> AddToWatchHistoryAsync(Guid userId, Guid mediaItemId);

	// Favorites
	Task<BaseResult<bool>> AddToFavoritesAsync(Guid userId, Guid mediaItemId);

	Task<BaseResult<List<MediaItemDto>>> GetFavoritesAsync(Guid userId, int pageNumber = 1, int pageSize = 10);

	// Want to Watch
	Task<BaseResult<bool>> AddToWantToWatchAsync(Guid userId, Guid mediaItemId);

	Task<BaseResult<List<MediaItemDto>>> GetWantToWatchAsync(Guid userId, int pageNumber = 1, int pageSize = 10);
}