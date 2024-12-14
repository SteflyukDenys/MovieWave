using System.ComponentModel.DataAnnotations;

namespace MovieWave.Domain.Dto.Comment;

public class AddCommentDto
{
	[Required]
	public Guid MediaItemId { get; set; }

	public Guid? ParentId { get; set; }

	[Required]
	[StringLength(500)]
	public string Text { get; set; }
}