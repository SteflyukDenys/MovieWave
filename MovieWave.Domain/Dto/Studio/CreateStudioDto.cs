using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Studio;

public class CreateStudioDto
{
	public string Name { get; set; }

	public string? Description { get; set; }

	public SeoAdditionInputDto SeoAddition { get; set; }
};
