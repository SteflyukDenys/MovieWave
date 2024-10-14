using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class RestrictedRating : NamedEntity<long>
{
	public string Slug { get; set; }
	public int Value { get; set; }
	public string Hint { get; set; }

	// Many-to-One
	public ICollection<MediaItem> MediaItems { get; set; }
}