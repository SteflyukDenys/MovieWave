using MovieWave.Domain.AbstractEntity;
using System.ComponentModel.DataAnnotations;

namespace MovieWave.Domain.Entity;

public class Review : AuditableEntity<Guid>
{
	public Guid MediaItemId { get; set; }
	public MediaItem MediaItem { get; set; }

	public Guid UserId { get; set; }
	public User User { get; set; }

	[Range(1, 10)]
	public int Rating { get; set; } // Rating BETWEEN 1 AND 10
	public string? Text { get; set; }
}
