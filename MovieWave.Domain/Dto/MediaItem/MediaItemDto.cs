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

namespace MovieWave.Domain.Dto.MediaItem
{
    public record MediaItemDto(
		Guid Id,
		string Name,
		string? OriginalName,
		string? Description,
		string? PosterPath,
		int? Duration,
		int? EpisodesCount,
		decimal? ImdbScore,
		string? FirstAirDate,
		string? LastAirDate,
		string? PublishedAt,
		MediaItemTypeDto MediaItemType,
		StatusDto Status,
		RestrictedRatingDto RestrictedRating,
		SeoAdditionDto SeoAddition,
		// One-to-Many
		List<SeasonDto> Seasons,
		List<EpisodeDto> Episodes,
		List<AttachmentDto> Attachments,
		List<ReviewDto> Reviews,
		List<NotificationDto> Notifications,
		List<CommentDto> Comments,
		// Many-to-Many
		List<CountryDto> Countries,
		List<StudioDto> Studios,
		List<TagDto> Tags,
		List<PersonDto> People
	);
}