using MovieWave.Domain.Dto.RestrictedRating;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface IRestrictedRatingService
{
	Task<CollectionResult<RestrictedRatingDto>> GetAllAsync();
	Task<BaseResult<RestrictedRatingDto>> GetByIdAsync(long id);
	Task<BaseResult<RestrictedRatingDto>> UpdateAsync(UpdateRestrictedRatingDto dto);
}