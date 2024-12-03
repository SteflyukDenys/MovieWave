namespace MovieWave.Domain.Dto.RestrictedRating;

public class RestrictedRatingDto
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Slug { get; set; }
	public int Value { get; set; }
	public string Hint { get; set; }
};