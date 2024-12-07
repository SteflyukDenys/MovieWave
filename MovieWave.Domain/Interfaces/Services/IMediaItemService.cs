using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IMediaItemService
{
	Task<CollectionResult<MediaItemDto>> GetMediaItemsAsync(int pageNumber = 1, int pageSize = 10);
	
	Task<BaseResult<MediaItemDto>> GetMediaItemByIdAsync(Guid id);

	Task<CollectionResult<MediaItemByTagDto>> GetMediaItemsByTagAsync(Guid tagId, int pageNumber = 1, int pageSize = 10);

	Task<BaseResult<MediaItemDto>> CreateMediaItemAsync(CreateMediaItemDto dto);

	Task<BaseResult<MediaItemDto>> DeleteMediaItemAsync(Guid id);

	Task<BaseResult<MediaItemDto>> UpdateMediaItemAsync(UpdateMediaItemDto dto);

	Task<CollectionResult<MediaItemDto>> SearchMediaItemsAsync(MediaItemSearchDto searchDto);
}

