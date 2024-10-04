using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Dto;

public record MediaItemDto(
		Guid Id,
		string Name,
		string? OriginalName,
		string? Description,
		MediaItemName MediaItemType,
		StatusType Status,
		string? PosterPath,
		int? Duration,
		string? FirstAirDate, //DateTime
		string? LastAirDate, //DateTime
		int? EpisodesCount,
		decimal? ImdbScore,
		string? PublishedAt //DateTime
	);
