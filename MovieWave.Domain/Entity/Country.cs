using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Country : NamedEntity<long>
{
	public SeoAddition? SeoAddition { get; set; }

	// Many-to-Many
	public ICollection<MediaItem> MediaItems { get; set; }
}
