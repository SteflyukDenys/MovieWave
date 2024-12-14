namespace MovieWave.Domain.Dto.Comment;

public class CommentDto
{
	public Guid Id { get; set; }

	public Guid CommentableId { get; set; }

	public string CommentableType { get; set; }

	public Guid UserId { get; set; }

	public string Username { get; set; }

	public string Text { get; set; }

	public DateTime CreatedAt { get; set; }

	public List<CommentDto> Children { get; set; }
}