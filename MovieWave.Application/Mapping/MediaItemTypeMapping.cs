using AutoMapper;
using MovieWave.Domain.Dto.MediaItemType;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class MediaItemTypeMapping : Profile
{
	public MediaItemTypeMapping()
	{
		CreateMap<MediaItemType, MediaItemTypeDto>()
			.ForMember(dest => dest.MediaItemName, opt => opt.MapFrom(src => src.MediaItemName.ToString()))
			.ReverseMap();
	}
}