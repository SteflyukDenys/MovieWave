using AutoMapper;
using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Dto.Comment;
using MovieWave.Domain.Dto.Country;
using MovieWave.Domain.Dto.Episode;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Dto.MediaItemType;
using MovieWave.Domain.Dto.Notification;
using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Dto.RestrictedRating;
using MovieWave.Domain.Dto.Review;
using MovieWave.Domain.Dto.Season;
using MovieWave.Domain.Dto.SeoAddition;
using MovieWave.Domain.Dto.Status;
using MovieWave.Domain.Dto.Studio;
using MovieWave.Domain.Dto.Tag;
using MovieWave.Domain.Entity;

namespace MovieWave.Application.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
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

			CreateMap<MediaItemType, MediaItemTypeDto>()
				.ForMember(dest => dest.MediaItemName, opt => opt.MapFrom(src => src.MediaItemName.ToString()))
				.ReverseMap();

			CreateMap<Status, StatusDto>()
				.ForMember(dest => dest.StatusType, opt => opt.MapFrom(src => src.StatusType.ToString()))
				.ReverseMap();

			CreateMap<SeoAddition, SeoAdditionDto>().ReverseMap();

			CreateMap<RestrictedRating, RestrictedRatingDto>().ReverseMap();

			CreateMap<Notification, NotificationDto>().ReverseMap();

			CreateMap<Review, ReviewDto>().ReverseMap();

			CreateMap<Attachment, AttachmentDto>().ReverseMap();

			CreateMap<Comment, CommentDto>().ReverseMap();

			CreateMap<Country, CountryDto>().ReverseMap();

			CreateMap<Episode, EpisodeDto>().ReverseMap();

			CreateMap<Person, PersonDto>().ReverseMap();

			CreateMap<Season, SeasonDto>().ReverseMap();

			CreateMap<Studio, StudioDto>().ReverseMap();

			CreateMap<Tag, TagDto>().ReverseMap();

		}
	}
}