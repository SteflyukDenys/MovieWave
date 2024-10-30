using MovieWave.Domain.Dto.Country;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services;

public interface ICountryService
{
	Task<CollectionResult<CountryDto>> GetAllAsync();

	Task<BaseResult<CountryDto>> GetByIdAsync(long id);

	Task<BaseResult<CountryDto>> CreateAsync(CreateCountryDto dto);

	Task<BaseResult<CountryDto>> DeleteAsync(long id);

	Task<BaseResult<CountryDto>> UpdateAsync(UpdateCountryDto dto);
}