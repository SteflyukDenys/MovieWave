using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Episode;

public class CreateEpisodeDto
{
	public Guid MediaItemId { get; set; }

	public Guid? SeasonId { get; set; }

	public string Name { get; set; }

	public string? Description { get; set; }

	public int? Duration { get; set; }

	public DateTime? AirDate { get; set; }

	public bool? IsFiller { get; set; } = false;

	public string? ImagePath { get; set; }

	public SeoAdditionInputDto? SeoAddition { get; set; }
}