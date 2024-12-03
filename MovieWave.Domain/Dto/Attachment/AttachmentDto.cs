using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto.Attachment;

public class AttachmentDto
{
	public Guid Id { get; set; }

	public Guid MediaItemId { get; set; }

	public AttachmentType AttachmentType { get; set; }

	public string AttachmentUrl { get; set; }
};