using MovieWave.Domain.Dto.MediaItemType;
using MovieWave.Domain.Dto.RestrictedRating;
using MovieWave.Domain.Dto.SeoAddition;
using MovieWave.Domain.Dto.Status;

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
	//// One-to-Many
	//List<SeasonDto> Seasons,
	//List<EpisodeDto> Episodes,
	//List<AttachmentDto> Attachments,
	//List<ReviewDto> Reviews,
	//List<NotificationDto> Notifications,
	//List<CommentDto> Comments,
	//// Many-to-Many
	//List<CountryDto> Countries,
	//List<StudioDto> Studios,
	//List<TagDto> Tags,
	//List<PersonDto> People
}