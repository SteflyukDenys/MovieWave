using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class SeoAddition : BaseEntity<long>
{
	public string Slug { get; set; }

	public string? MetaTitle { get; set; }
	public string? Description { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaImagePath { get; set; }

	public Guid? SeoableId { get; set; } // Id of the entity to which SEO data is linked
	public string? SeoableType { get; set; } // Entity type ("MediaItem", "Tag"...)
}