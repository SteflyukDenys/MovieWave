using AutoMapper;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping;

public class MediaItemMapping : Profile
{
	public MediaItemMapping()
	{
		CreateMap<MediaItem, MediaItemDto>()
			.ForMember(dest => dest.FirstAirDate, opt => opt.MapFrom(src => src.FirstAirDate))
			.ForMember(dest => dest.LastAirDate, opt => opt.MapFrom(src => src.LastAirDate))
			.ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt))
			.ReverseMap();

		CreateMap<CreateMediaItemDto, MediaItem>()
			.ForMember(dest => dest.Tags, opt => opt.Ignore())
			.ForMember(dest => dest.FirstAirDate, opt => opt.MapFrom(src => src.FirstAirDate.HasValue ? DateTime.SpecifyKind(src.FirstAirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
			.ForMember(dest => dest.LastAirDate, opt => opt.MapFrom(src => src.LastAirDate.HasValue ? DateTime.SpecifyKind(src.LastAirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
			.ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt.HasValue ? DateTime.SpecifyKind(src.PublishedAt.Value, DateTimeKind.Utc) : (DateTime?)null));

		CreateMap<UpdateMediaItemDto, MediaItem>()
			.ForMember(dest => dest.FirstAirDate, opt => opt.MapFrom(src => src.FirstAirDate.HasValue ? DateTime.SpecifyKind(src.FirstAirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
			.ForMember(dest => dest.LastAirDate, opt => opt.MapFrom(src => src.LastAirDate.HasValue ? DateTime.SpecifyKind(src.LastAirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
			.ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt.HasValue ? DateTime.SpecifyKind(src.PublishedAt.Value, DateTimeKind.Utc) : (DateTime?)null));

		CreateMap<MediaItemSearchDto, MediaItem>();
	}
}