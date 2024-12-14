using MovieWave.Domain.Dto.Season;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface ISeasonService
{
	Task<BaseResult<SeasonDto>> CreateSeasonAsync(CreateSeasonDto dto);

	Task<BaseResult<SeasonDto>> UpdateSeasonAsync(UpdateSeasonDto dto);

	Task<BaseResult> DeleteSeasonAsync(Guid seasonId);

	Task<BaseResult<SeasonDto>> GetSeasonByIdAsync(Guid seasonId);
}