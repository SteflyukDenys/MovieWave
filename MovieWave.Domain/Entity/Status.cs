using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

// TPH
public abstract class Status : NamedEntity<long>
{
	public string? Description { get; set; }

	public int? SeoAdditionId { get; set; }
	public SeoAddition? SeoAddition { get; set; }

	public ICollection<MediaItem> MediaItems { get; set; }
}
