using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Tag;

public class UpdateTagDto
{
	public long Id { get; set; }

	public string Name { get; set; }

	public string? Description { get; set; }

	public bool IsGenre { get; set; }

	public long? ParentId { get; set; }

	public SeoAdditionInputDto SeoAddition { get; set; } = new SeoAdditionInputDto();
}