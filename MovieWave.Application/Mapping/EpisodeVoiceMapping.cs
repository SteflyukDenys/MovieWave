using AutoMapper;
using MovieWave.Domain.Dto.EpisodeVoice;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class EpisodeVoiceMapping : Profile
{
	public EpisodeVoiceMapping()
	{
		CreateMap<EpisodeVoice, EpisodeVoiceDto>().ReverseMap();

		CreateMap<CreateEpisodeVoiceDto, EpisodeVoice>()
			.ReverseMap();
	}
}