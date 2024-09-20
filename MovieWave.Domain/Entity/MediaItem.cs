using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class MediaItem : NamedAuditableEntity<Guid>
{
	public int MediaItemTypeId { get; set; }
	public MediaItemType MediaItemType { get; set; }

	public string OriginalName { get; set; }
	public string Description { get; set; }

	public int? StatusId { get; set; }
	public Status Status { get; set; }

	public int? RestrictedRatingId { get; set; }
	public RestrictedRating RestrictedRating { get; set; }

	public string PosterPath { get; set; }
	public int? Duration { get; set; } // In minutes
	public DateTime? FirstAirDate { get; set; }
	public DateTime? LastAirDate { get; set; }
	public int? EpisodesCount { get; set; }
	public decimal? ImdbScore { get; set; }
	public DateTime? PublishedAt { get; set; }

	public int SeoAdditionId { get; set; }
	public SeoAddition SeoAddition { get; set; }

	// One-to-Many
	public ICollection<Season> Seasons { get; set; }
	public ICollection<Episode> Episodes { get; set; }
	public ICollection<Attachment> Attachments { get; set; }
	public ICollection<Review> Reviews { get; set; }
	public ICollection<Notification> Notifications { get; set; }
	public ICollection<Comment> Comments { get; set; }

	// Many-to-Many
	public ICollection<UserMediaItemList> UserMovieLists { get; set; }
	public ICollection<MediaItemPerson> MoviePeople { get; set; }
}
