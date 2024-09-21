using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class SeoAddition : BaseEntity<int>
{
	public string Slug { get; set; }

	public string MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaImagePath { get; set; }

	// One-to-one
	public MediaItem MediaItem { get; set; }
	public MediaItemType? MediaItemsType { get; set; }
	public RestrictedRating? RestrictedRating { get; set; }
	public Episode? Episode { get; set; }
	public Country? Country { get; set; }
	public Studio? Studio { get; set; }
	public Person? Person { get; set; }
	public Notification? Notification { get; set; }
	public Tag? Tag { get; set; }

}