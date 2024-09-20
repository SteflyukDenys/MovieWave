using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Studio : NamedEntity<Guid>
{
	public string? LogoPath { get; set; }
	public string? Description { get; set; }

	public int? SeoAdditionId { get; set; }
	public SeoAddition SeoAddition { get; set; }
}