﻿namespace MovieWave.Domain.Dto.Banner;

public class UpdateBannerDto
{
	public Guid Id { get; set; }

	public string Title { get; set; }

	public string? Description { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public int DisplayOrder { get; set; }
}