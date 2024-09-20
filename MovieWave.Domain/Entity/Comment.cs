using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Comment : AuditableEntity<Guid>
{
	public Guid? CommentableId { get; set; } // ID of the entity
	public string? CommentableType { get; set; } // Entity Type ("MediaItem" or "Episode")

	public Guid UserId { get; set; }
	public User User { get; set; }

	public Guid? ParentId { get; set; }
	public Comment? Parent { get; set; }
	public List<Comment> Children { get; set; }

	public string? Text { get; set; }
}
