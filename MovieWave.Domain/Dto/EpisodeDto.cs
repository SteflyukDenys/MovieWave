namespace MovieWave.Domain.Dto;

public record EpisodeDto(
		Guid Id,
		Guid MediaItemId,
		Guid SeasonId,
		string? Name,
		string? Description,
		int? Duration,
		string? AirDate, //DateTime
		bool IsFiller,
		string? ImagePath
	);
