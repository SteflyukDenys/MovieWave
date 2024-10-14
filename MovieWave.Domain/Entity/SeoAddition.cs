using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class SeoAddition
{
	public string Slug { get; set; }
	public string? MetaTitle { get; set; }
	public string? Description { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaImagePath { get; set; }
}

// Country
// Episode
// MediaItem
// MediaItemType
// Person
// Studio
// Tag