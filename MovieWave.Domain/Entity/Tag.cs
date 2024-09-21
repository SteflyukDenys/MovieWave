using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Tag : NamedEntity<Guid>
{
	public string? Description { get; set; }
	public bool IsGenre { get; set; } // default 0

	public Guid? ParentId { get; set; }
	public Tag? Parent { get; set; }
	public List<Tag> Children { get; set; }

	public int? SeoAdditionId { get; set; }
	public SeoAddition? SeoAddition { get; set; }

	// Many-to-Many
	public ICollection<MediaItem> MediaItems { get; set; }
}
