using MovieWave.Domain.Dto.Status;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IStatusService
{
	Task<CollectionResult<StatusDto>> GetAllAsync();

	Task<BaseResult<StatusDto>> GetByIdAsync(long id);

	Task<BaseResult<StatusDto>> UpdateAsync(UpdateStatusDto dto);
}