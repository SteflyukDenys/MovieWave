﻿using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class Status : BaseEntity<long>
{
	public StatusType StatusType { get; set; }
	public string? Description { get; set; }

	public ICollection<MediaItem> MediaItems { get; set; }
}
