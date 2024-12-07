using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Dto.Comment;
using MovieWave.Domain.Dto.Country;
using MovieWave.Domain.Dto.Episode;
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

namespace MovieWave.Domain.Dto.MediaItem;

public class MediaItemDto{
	public Guid Id {get; set; }
	public string Name {get; set; }
	public string? OriginalName { get; set; }
	public string? Description { get; set; }
	public int? Duration { get; set; }
	public int? EpisodesCount { get; set; }
	public decimal? ImdbScore { get; set; }

	public string? FirstAirDate { get; set; }
	public string? LastAirDate { get; set; }
	public string? PublishedAt { get; set; }

	public MediaItemTypeDto MediaItemType = new MediaItemTypeDto();
	public StatusDto Status = new StatusDto();
	public RestrictedRatingDto RestrictedRating = new RestrictedRatingDto();

	public SeoAdditionInputDto SeoAddition = new SeoAdditionInputDto();

	List<SeasonDto> Seasons { get; set; }
	List<EpisodeDto> Episodes { get; set; }
	List<AttachmentDto> Attachments { get; set; }
	List<ReviewDto> Reviews { get; set; }
	List<NotificationDto> Notifications { get; set; }
	List<CommentDto> Comments { get; set; }
	// Many-to-Many
	List<CountryDto> Countries { get; set; }
	List<StudioDto> Studios { get; set; }
	List<TagDto> Tags { get; set; }
	List<PersonDto> People { get; set; }
}