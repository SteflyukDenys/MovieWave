using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

// If a film, then one series
public class Episode : NamedAuditableEntity<Guid>
{
	public Guid MediaItemId { get; set; }
	public MediaItem MediaItem { get; set; }

	public Guid SeasonId { get; set; }
	public Season Season { get; set; }

	public string? Description { get; set; }
	public int? Duration { get; set; } // In minutes
	public DateTime? AirDate { get; set; }
	public bool? IsFiller { get; set; } // default false
	public string? ImagePath { get; set; }

	public SeoAddition SeoAddition { get; set; } = new SeoAddition();

	public ICollection<EpisodeVoice> EpisodeVoices { get; set; }
	public ICollection<Notification> Notifications { get; set; }
	public ICollection<Comment> Comments { get; set; }
}
