using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IRatingsService
{
	Task<BaseResult<bool>> RateMediaItemAsync(Guid userId, RateMediaItemDto dto);

	Task<BaseResult<double>> GetAverageRatingAsync(Guid mediaItemId);
}