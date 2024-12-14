using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Tag;

public class TagDto
{
	public long Id { get; set; }

	public string Name { get; set; }

	public string? Description { get; set; }

	public bool IsGenre { get; set; }

	public long? ParentId { get; set; }

	public SeoAdditionDto SeoAddition { get; set; } = new SeoAdditionDto();

	public List<TagDto>? Children { get; set; }
}