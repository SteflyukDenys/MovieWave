using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IMediaItemService
{
	Task<CollectionResult<MediaItemDto>> GetMediaItemsAsync(int pageNumber = 1, int pageSize = 10);
	
	Task<BaseResult<MediaItemDto>> GetMediaItemByIdAsync(Guid id);

	Task<CollectionResult<MediaItemDto>> GetAllMoviesAsync(int pageNumber = 1, int pageSize = 10);

	Task<CollectionResult<MediaItemDto>> GetAllSeriesAsync(int pageNumber = 1, int pageSize = 10);

	Task<CollectionResult<MediaItemByTagDto>> GetMoviesByTagAsync(long tagId, int pageNumber = 1, int pageSize = 10);

	Task<CollectionResult<MediaItemByTagDto>> GetSeriesByTagAsync(long tagId, int pageNumber = 1, int pageSize = 10);

	Task<BaseResult<MediaItemDto>> CreateMediaItemAsync(CreateMediaItemDto dto);

	Task<BaseResult<MediaItemDto>> DeleteMediaItemAsync(Guid id);

	Task<BaseResult<MediaItemDto>> UpdateMediaItemAsync(UpdateMediaItemDto dto);

	Task<CollectionResult<MediaItemDto>> SearchMediaItemsAsync(MediaItemSearchDto searchDto, int pageNumber = 1, int pageSize = 10);

	bool CheckSlugExists(string slug);

	Task<bool> ExistsByNameAndYearAsync(string name, int? year);
}

