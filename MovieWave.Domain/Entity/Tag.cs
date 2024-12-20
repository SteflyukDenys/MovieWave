﻿using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Tag : NamedEntity<long>
{
	public string? Description { get; set; }
	public bool IsGenre { get; set; } // default 0

	public long? ParentId { get; set; }
	public Tag? Parent { get; set; }
	public List<Tag>? Children { get; set; }

	public SeoAddition SeoAddition { get; set; } = new SeoAddition();

	// Many-to-Many relationship with MediaItem
	public ICollection<MediaItem> MediaItems { get; set; } = new List<MediaItem>();
}
