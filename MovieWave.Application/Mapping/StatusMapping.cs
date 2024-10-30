using AutoMapper;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Dto.Status;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class StatusMapping : Profile
{
	public StatusMapping()
	{
		CreateMap<Status, StatusDto>()
			.ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => src.StatusType.ToString()))
			.ReverseMap();

		CreateMap<UpdateStatusDto, Status>()
			.ReverseMap();
	}
}