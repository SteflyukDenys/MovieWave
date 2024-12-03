namespace MovieWave.Domain.Dto.Attachment;

public class CreateAttachmentDto
{
	public Guid MediaItemId { get; set; }

	public int AttachmentType { get; set; }

}