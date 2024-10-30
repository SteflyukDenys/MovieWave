using AutoMapper;
using MovieWave.Domain.Dto.Studio;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class StudioMapping : Profile
{
	public StudioMapping()
	{
		CreateMap<Studio, StudioDto>().ReverseMap();

		CreateMap<UpdateStudioDto, Studio>().ReverseMap();

		CreateMap<CreateStudioDto, Studio>().ReverseMap();
	}
}