using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Review : AuditableEntity<Guid>
{
	public Guid MediaItemId { get; set; }
	public MediaItem MediaItem { get; set; }

	public Guid UserId { get; set; }
	public User User { get; set; }

	public int Rating { get; set; } // Rating BETWEEN 1 AND 10
	public string Text { get; set; }
}
