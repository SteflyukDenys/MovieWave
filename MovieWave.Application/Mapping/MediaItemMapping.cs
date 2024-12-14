using AutoMapper;
using MovieWave.Domain.Dto.Comment;
using MovieWave.Domain.Dto.Country;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Dto.Role;
using MovieWave.Domain.Dto.Studio;
using MovieWave.Domain.Dto.Tag;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping
{
	public class MediaItemMapping : Profile
	{
		public MediaItemMapping()
		{
			CreateMap<MediaItem, MediaItemDto>()
				.ForMember(dest => dest.FirstAirDate, opt => opt.MapFrom(src => src.FirstAirDate))
				.ForMember(dest => dest.LastAirDate, opt => opt.MapFrom(src => src.LastAirDate))
				.ForMember(dest => dest.Countries, opt => opt.MapFrom(src => src.Countries))
				.ForMember(dest => dest.Studios, opt => opt.MapFrom(src => src.Studios))
				.ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
				.ForMember(dest => dest.People, opt => opt.MapFrom(src => src.MediaItemPeople.Select(mp => mp.Person)))
				.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.MediaItemPeople))
				.ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
				.ReverseMap();

			CreateMap<CreateMediaItemDto, MediaItem>()
				.ForMember(dest => dest.Tags, opt => opt.Ignore())
				.ForMember(dest => dest.FirstAirDate, opt => opt.MapFrom(src => src.FirstAirDate.HasValue ? DateTime.SpecifyKind(src.FirstAirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
				.ForMember(dest => dest.LastAirDate, opt => opt.MapFrom(src => src.LastAirDate.HasValue ? DateTime.SpecifyKind(src.LastAirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
				.ReverseMap();

			CreateMap<UpdateMediaItemDto, MediaItem>()
				.ForMember(dest => dest.FirstAirDate, opt => opt.MapFrom(src => src.FirstAirDate.HasValue ? DateTime.SpecifyKind(src.FirstAirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
				.ForMember(dest => dest.LastAirDate, opt => opt.MapFrom(src => src.LastAirDate.HasValue ? DateTime.SpecifyKind(src.LastAirDate.Value, DateTimeKind.Utc) : (DateTime?)null))
				.ReverseMap();

			CreateMap<MediaItemSearchDto, MediaItem>();
		}
	}
}
