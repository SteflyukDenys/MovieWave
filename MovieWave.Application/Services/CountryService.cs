using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Country;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;

namespace MovieWave.Application.Services;

public class CountryService : ICountryService
{
	private readonly IBaseRepository<Country> _countryRepository;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public CountryService(IBaseRepository<Country> countryRepository, ILogger logger, IMapper mapper)
	{
		_countryRepository = countryRepository;
		_logger = logger;
		_mapper = mapper;
	}

	public async Task<CollectionResult<CountryDto>> GetAllAsync()
	{
		List<CountryDto> countries;

		countries = await _countryRepository.GetAll()
			.Include(c => c.SeoAddition)
			.Select(c => _mapper.Map<CountryDto>(c))
			.ToListAsync();

		if (!countries.Any())
		{
			_logger.Warning(ErrorMessage.CountriesNotFound);
			return new CollectionResult<CountryDto>
			{
				ErrorMessage = ErrorMessage.CountriesNotFound,
				ErrorCode = (int)ErrorCodes.CountriesNotFound
			};
		}

		return new CollectionResult<CountryDto> { Data = countries, Count = countries.Count };
	}

	public async Task<BaseResult<CountryDto>> GetByIdAsync(long id)
	{
		CountryDto countryDto;

		var country = await _countryRepository.GetAll()
			.Include(c => c.SeoAddition)
			.FirstOrDefaultAsync(c => c.Id == id);

		if (country == null)
		{
			_logger.Warning($"Country {id} not found");
			return new BaseResult<CountryDto>
			{
				ErrorMessage = ErrorMessage.CountryNotFound,
				ErrorCode = (int)ErrorCodes.CountryNotFound
			};
		}

		countryDto = _mapper.Map<CountryDto>(country);

		return new BaseResult<CountryDto> { Data = countryDto };
	}

	public async Task<BaseResult<CountryDto>> CreateAsync(CreateCountryDto dto)
	{
		var existingCountry = await _countryRepository.GetAll()
			.FirstOrDefaultAsync(c => c.Name == dto.Name);

		if (existingCountry != null)
		{
			return new BaseResult<CountryDto>
			{
				ErrorMessage = ErrorMessage.CountryAlreadyExists,
				ErrorCode = (int)ErrorCodes.CountryAlreadyExists
			};
		}

		var newCountry = _mapper.Map<Country>(dto);
		await _countryRepository.CreateAsync(newCountry);
		await _countryRepository.SaveChangesAsync();

		return new BaseResult<CountryDto> { Data = _mapper.Map<CountryDto>(newCountry) };
	}

	public async Task<BaseResult<CountryDto>> DeleteAsync(long id)
	{
		var country = await _countryRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id);
		if (country == null)
		{
			return new BaseResult<CountryDto>
			{
				ErrorMessage = ErrorMessage.CountryNotFound,
				ErrorCode = (int)ErrorCodes.CountryNotFound
			};
		}

		_countryRepository.Remove(country);
		await _countryRepository.SaveChangesAsync();

		return new BaseResult<CountryDto> { Data = _mapper.Map<CountryDto>(country) };
	}

	public async Task<BaseResult<CountryDto>> UpdateAsync(UpdateCountryDto dto)
	{
		var country = await _countryRepository.GetAll().FirstOrDefaultAsync(c => c.Id == dto.Id);
		if (country == null)
		{
			return new BaseResult<CountryDto>
			{
				ErrorMessage = ErrorMessage.CountryNotFound,
				ErrorCode = (int)ErrorCodes.CountryNotFound
			};
		}

		_mapper.Map(dto, country);
		var updatedCountry = _countryRepository.Update(country);
		await _countryRepository.SaveChangesAsync();

		return new BaseResult<CountryDto> { Data = _mapper.Map<CountryDto>(updatedCountry) };
	}
}
