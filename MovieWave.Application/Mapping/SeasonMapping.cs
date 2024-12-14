using AutoMapper;
using MovieWave.Domain.Dto.Season;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class SeasonMapping : Profile
{
	public SeasonMapping()
	{
		CreateMap<Season, SeasonDto>().ReverseMap();

		CreateMap<CreateSeasonDto, Season>()
			.ReverseMap();

		CreateMap<UpdateSeasonDto, Season>()
			.ReverseMap();
	}
}