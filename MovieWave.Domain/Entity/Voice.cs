﻿using MovieWave.Domain.AbstractEntity;

namespace MovieWave.Domain.Entity;

public class Voice : NamedEntity<Guid>
{
	public string? Description { get; set; }
	public string Locale { get; set; }
	public string? IconPath { get; set; }

	public int? SeoAdditionId { get; set; }
	public SeoAddition SeoAddition { get; set; }


	public ICollection<EpisodeVoice> EpisodeVoices { get; set; }
}