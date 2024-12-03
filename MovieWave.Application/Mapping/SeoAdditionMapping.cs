using AutoMapper;
using MovieWave.Domain.Dto.SeoAddition;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class SeoAdditionMapping : Profile
{
	public SeoAdditionMapping()
	{
		CreateMap<SeoAddition, SeoAdditionDto>();

		CreateMap<SeoAdditionInputDto, SeoAddition>().ReverseMap();
	}
}