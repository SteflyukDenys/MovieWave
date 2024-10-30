using AutoMapper;
using MovieWave.Domain.Dto.Country;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class CountryMapping : Profile
{
	public CountryMapping()
	{
		CreateMap<Country, CountryDto>().ReverseMap();

		CreateMap<CreateCountryDto, Country>().ReverseMap();

		CreateMap<UpdateCountryDto, CountryDto>().ReverseMap();
	}
}