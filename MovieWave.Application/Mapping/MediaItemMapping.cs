using AutoMapper;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class MediaItemMapping : Profile
{
	public MediaItemMapping()
	{
		CreateMap<MediaItem, MediaItemDto>()
			.ForMember(dest => dest.FirstAirDate, opt => opt.MapFrom(src => src.FirstAirDate.HasValue ? src.FirstAirDate.Value.ToString("yyyy-MM-dd") : null))
			.ForMember(dest => dest.LastAirDate, opt => opt.MapFrom(src => src.LastAirDate.HasValue ? src.LastAirDate.Value.ToString("yyyy-MM-dd") : null))
			.ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt.HasValue ? src.PublishedAt.Value.ToString("yyyy-MM-dd") : null))
			.ReverseMap();

		CreateMap<CreateMediaItemDto, MediaItem>()
			.ReverseMap();

		CreateMap<UpdateMediaItemDto, MediaItem>()
			.ReverseMap();
	}
}