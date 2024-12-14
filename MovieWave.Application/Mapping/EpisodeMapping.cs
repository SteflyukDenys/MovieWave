using AutoMapper;
using MovieWave.Domain.Dto.Episode;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class EpisodeMapping : Profile
{
	public EpisodeMapping()
	{
		CreateMap<Episode, EpisodeDto>().ReverseMap();

		CreateMap<CreateEpisodeDto, Episode>()
			.ForMember(dest => dest.AirDate, opt => opt.MapFrom(src => src.AirDate.HasValue ? DateTime.SpecifyKind(src.AirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
			.ReverseMap();

		CreateMap<UpdateEpisodeDto, Episode>()
			.ForMember(dest => dest.AirDate, opt => opt.MapFrom(src => src.AirDate.HasValue ? DateTime.SpecifyKind(src.AirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
			.ReverseMap();
	}
}