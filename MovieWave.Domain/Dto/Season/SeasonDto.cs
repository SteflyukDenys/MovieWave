namespace MovieWave.Domain.Dto.Season;

public class SeasonDto
{
	public Guid Id { get; set; }

	public Guid MediaItemId { get; set; }

	public string Name { get; set; }
}