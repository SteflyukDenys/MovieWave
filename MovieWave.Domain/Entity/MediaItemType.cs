using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class MediaItemType : BaseEntity<int>
{
	public MediaItemName MediaItemName { get; set; }

	public SeoAddition SeoAddition { get; set; } = new SeoAddition();

	// Many-to-One
	public ICollection<MediaItem> MediaItems { get; set; }
}
