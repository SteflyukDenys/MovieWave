using MovieWave.Domain.AbstractEntity;
using NpgsqlTypes;

namespace MovieWave.Domain.Entity;

public class MediaItem : NamedAuditableEntity<Guid>
{
	public MediaItem()
	{
		Id = Guid.NewGuid();
	}
	public int MediaItemTypeId { get; set; }
	public MediaItemType MediaItemType { get; set; }

	public string OriginalName { get; set; }
	public string Description { get; set; }

	public long? StatusId { get; set; }
	public Status Status { get; set; }

	public long? RestrictedRatingId { get; set; }
	public RestrictedRating RestrictedRating { get; set; }
	
	public int? Duration { get; set; } // In minutes
	public DateTime? FirstAirDate { get; set; }
	public DateTime? LastAirDate { get; set; }
	public int? EpisodesCount { get; set; }
	public decimal? ImdbScore { get; set; }
	public DateTime? PublishedAt { get; set; }

	// Composition
	public SeoAddition? SeoAddition { get; set; } = new SeoAddition();

	public NpgsqlTsVector SearchVector { get; set; }

	// One-to-Many
	public ICollection<Season> Seasons { get; set; }
	public ICollection<Episode> Episodes { get; set; }
	public ICollection<Attachment> Attachments { get; set; }
	public ICollection<Review> Reviews { get; set; }
	public ICollection<Notification> Notifications { get; set; }
	public ICollection<Comment> Comments { get; set; }
	public ICollection<UserMediaItemList> UserMediaItemLists { get; set; }

	// Many-to-Many
	public ICollection<Country> Countries { get; set; }
	public ICollection<Studio> Studios { get; set; }
	public ICollection<Tag> Tags { get; set; }
	public ICollection<Person> People { get; set; }
	public ICollection<MediaItemPerson> MediaItemPeople { get; set; }
}
