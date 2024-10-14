using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Country : NamedEntity<Guid>
{
	public SeoAddition? SeoAddition { get; set; } = new SeoAddition();

	// Many-to-Many
	public ICollection<MediaItem> MediaItems { get; set; }
}
