using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Studio;

public class StudioDto
{
	public long Id { get; set; }

	public string Name { get; set; }

	public string? LogoPath { get; set; }

	public string? Description { get; set; }

	public SeoAdditionDto SeoAddition = new SeoAdditionDto();
}