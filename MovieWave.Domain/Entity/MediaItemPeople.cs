using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class MediaItemPeople
{
	public Guid MediaItemId { get; set; }
	public MediaItem MediaItem { get; set; }

	public Guid PersonId { get; set; }
	public Person Person { get; set; }

	public PersonRole PersonRole { get; set; }
}