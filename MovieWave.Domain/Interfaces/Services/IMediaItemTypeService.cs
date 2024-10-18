using MovieWave.Domain.Dto.MediaItemType;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IMediaItemTypeService
{
	Task<CollectionResult<MediaItemTypeDto>> GetAllAsync();

	Task<BaseResult<MediaItemTypeDto>> GetByIdAsync(int id);
}