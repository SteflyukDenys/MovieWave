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
	public double? ImdbScore { get; set; }

	public string? FirstAirDate { get; set; }
	public string? LastAirDate { get; set; }

	public MediaItemTypeDto MediaItemType = new MediaItemTypeDto();
	public StatusDto Status = new StatusDto();
	public RestrictedRatingDto RestrictedRating = new RestrictedRatingDto();

	public SeoAdditionInputDto SeoAddition = new SeoAdditionInputDto();

	public List<SeasonDto> Seasons { get; set; }
	public List<EpisodeDto> Episodes { get; set; }
	public List<AttachmentDto> Attachments { get; set; }
	public List<ReviewDto> Reviews { get; set; }
	public List<NotificationDto> Notifications { get; set; }
	public List<CommentDto> Comments { get; set; }
	// Many-to-Many
	public List<CountryDto> Countries { get; set; }
	public List<StudioDto> Studios { get; set; }
	public List<TagDto> Tags { get; set; }
	public List<PersonDto> People { get; set; }
	public List<PersonRolesDto> Roles { get; set; }
}