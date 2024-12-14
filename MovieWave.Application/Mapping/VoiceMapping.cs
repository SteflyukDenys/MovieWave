using AutoMapper;
using MovieWave.Domain.Dto.Voice;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class VoiceMapping : Profile
{
	public VoiceMapping()
	{
		CreateMap<Voice, VoiceDto>().ReverseMap();

		CreateMap<CreateVoiceDto, Voice>()
			.ReverseMap();

		CreateMap<UpdateVoiceDto, Voice>()
			.ReverseMap();
	}
}