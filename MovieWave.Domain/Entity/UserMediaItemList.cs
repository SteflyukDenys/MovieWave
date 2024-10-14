using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class UserMediaItemList
{
	public Guid UserId { get; set; }
	public User User { get; set; }

	public Guid MediaItemId { get; set; }
	public MediaItem MediaItem { get; set; }

	public ListType ListTypeId { get; set; }
}
