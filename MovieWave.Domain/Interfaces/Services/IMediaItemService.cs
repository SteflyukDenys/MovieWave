using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IMediaItemService
{
	Task<CollectionResult<MediaItemDto>> GetMediaItemsAsync();
	
	Task<BaseResult<MediaItemDto>> GetMediaItemByIdAsync(Guid id);

	Task<BaseResult<MediaItemDto>> CreateMediaItemAsync(CreateMediaItemDto dto);

	Task<BaseResult<MediaItemDto>> DeleteMediaItemAsync(Guid id);

	Task<BaseResult<MediaItemDto>> UpdateMediaItemAsync(UpdateMediaItemDto dto);

	//Task<CollectionResult<MediaItemDto>> SearchMediaItemsAsync(MediaItemSearchDto searchDto);
}

