using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class Notification : AuditableEntity<Guid>
{
	public Notification()
	{
		Id = Guid.NewGuid();
	}

	public Guid UserId { get; set; }
	public User User { get; set; }

	public Guid? MediaItemId { get; set; }
	public MediaItem MediaItem { get; set; }

	public Guid? EpisodeId { get; set; }
	public Episode Episode { get; set; }

	public NotificationType NotificationType { get; set; } // enum
	public bool IsRead { get; set; } // default false
}
