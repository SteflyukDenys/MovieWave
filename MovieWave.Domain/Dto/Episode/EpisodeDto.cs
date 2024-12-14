using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Episode;

public class EpisodeDto
{
	public Guid Id { get; set; }

	public Guid MediaItemId { get; set; }

	public Guid? SeasonId { get; set; }

	public string Name { get; set; }

	public string? Description { get; set; }

	public int? Duration { get; set; }

	public DateTime? AirDate { get; set; }

	public bool? IsFiller { get; set; }

	public string? ImagePath { get; set; }

	SeoAdditionDto seoAddition {get; set; }
}