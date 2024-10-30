using AutoMapper;
using MovieWave.Domain.Dto.Episode;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class EpisodeMapping : Profile
{
	public EpisodeMapping()
	{
		CreateMap<Episode, EpisodeDto>().ReverseMap();

		//CreateMap<CreateEpisodeDto, Episode>()
		//	.AfterMap((src, dest) => dest.Id = Guid.NewGuid())
		//	.ReverseMap();
	}
}