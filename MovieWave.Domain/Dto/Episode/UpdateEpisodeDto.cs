using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Episode;

public class UpdateEpisodeDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string? Description { get; set; }

	public int? Duration { get; set; }

	public DateTime? AirDate { get; set; }

	public bool? IsFiller { get; set; }

	public string? ImagePath { get; set; }

	SeoAdditionInputDto? seoAddition { get; set; }

}
