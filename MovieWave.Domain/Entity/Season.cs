using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Season : NamedEntity<Guid>
{
	public Guid MediaItemId { get; set; }
	public MediaItem MediaItem { get; set; }

	public ICollection<Episode> Episodes { get; set; }
}
