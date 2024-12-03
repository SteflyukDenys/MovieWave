using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Tag;

public class CreateTagDto
{
	public string Name { get; set; }
	public string? Description { get; set; }
	public bool IsGenre { get; set; }
	public Guid? ParentId { get; set; }
	public SeoAdditionInputDto SeoAddition { get; set; } = new SeoAdditionInputDto();
}