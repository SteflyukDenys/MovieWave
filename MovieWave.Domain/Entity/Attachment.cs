using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class Attachment : AuditableEntity<Guid>
{
	public Attachment()
	{
		Id = Guid.NewGuid();
	}
	public Guid MediaItemId { get; set; }
	public MediaItem MediaItem { get; set; }

	public AttachmentType AttachmentType { get; set; } // enum
	public string AttachmentUrl { get; set; }
}
