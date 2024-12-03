using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.MediaItem;

public record UpdateMediaItemDto(
	Guid Id,
	string Name,
	string? OriginalName,
	string? Description,
	int? Duration,
	int? EpisodesCount,
	decimal? ImdbScore,

	int MediaItemTypeId,
	long? StatusId,
	long? RestrictedRatingId,

	DateTime? FirstAirDate,
	DateTime? LastAirDate,
	DateTime? PublishedAt,
	SeoAdditionInputDto SeoAddition
);