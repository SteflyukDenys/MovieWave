using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class SeoAddition : BaseEntity<int>
{
	public string Slug { get; set; }

	public string MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public string? MetaImagePath { get; set; }

	// One-to-one
	public MediaItem MediaItems { get; set; }
	public MediaItemType? MediaItemsType { get; set; }
	public RestrictedRating? RestrictedRatings { get; set; }
	public Episode? Episodes { get; set; }
	public Country? Countries { get; set; }
	public Studio? Studios { get; set; }
	public Person? Peoples { get; set; }
	public Notification? Notifications { get; set; }
	public Tag? Tags { get; set; }

}