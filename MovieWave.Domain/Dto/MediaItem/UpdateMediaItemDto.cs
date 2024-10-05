namespace MovieWave.Domain.Dto.MediaItem;

public record UpdateMediaItemDto(
		Guid id,
		string Name,
		string? OriginalName,
		string? Description,
		string? PosterPath,
		int? Duration,
		int? EpisodesCount,
		decimal? ImdbScore
);
