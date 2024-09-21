using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class RestrictedRating : NamedEntity<long>
{
	public string Slug { get; set; }
	public int Value { get; set; }
	public string Hint { get; set; }

	public int? SeoAdditionId { get; set; }
	public SeoAddition SeoAddition { get; set; }

	// Many-to-One
	public ICollection<MediaItem> MediaItems { get; set; }
}