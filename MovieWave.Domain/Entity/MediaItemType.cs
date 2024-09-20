using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class MediaItemType : BaseEntity<int>
{
	public MediaItemName Name { get; set; }

	public int? SeoAdditionId { get; set; }
	public SeoAddition SeoAddition { get; set; }

	// Many-to-One
	public ICollection<MediaItem> MediaItems { get; set; }
}
