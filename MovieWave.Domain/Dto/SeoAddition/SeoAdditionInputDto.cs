namespace MovieWave.Domain.Dto.SeoAddition;

public class SeoAdditionInputDto
{
	public string Slug { get; set; }
	public string? MetaTitle { get; set; }
	public string? Description { get; set; }
	public string? MetaDescription { get; set; }
	// MetaImagePath тут немає
}