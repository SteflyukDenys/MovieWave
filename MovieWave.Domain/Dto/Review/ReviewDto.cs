namespace MovieWave.Domain.Dto.Review;

public class ReviewDto
{
	public Guid Id { get; set; }

	public Guid MediaItemId { get; set; }

	public Guid UserId { get; set; }

	public string Username { get; set; }

	public int Rating { get; set; }

	public string? Text { get; set; }

	public DateTime CreatedAt { get; set; }
}