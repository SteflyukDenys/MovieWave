using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Dto.Studio;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IStudioService
{
	Task<CollectionResult<StudioDto>> GetAllAsync();

	Task<BaseResult<StudioDto>> GetByIdAsync(long id);

	Task<CollectionResult<Studio>> GetStudioByIdsAsync(List<long> studioIds);

	Task<BaseResult<StudioDto>> CreateAsync(CreateStudioDto dto, FileDto logoPath);

	Task<BaseResult<StudioDto>> UpdateAsync(UpdateStudioDto dto, FileDto newLogoPath);

	Task<BaseResult<StudioDto>> DeleteAsync(long id);
}