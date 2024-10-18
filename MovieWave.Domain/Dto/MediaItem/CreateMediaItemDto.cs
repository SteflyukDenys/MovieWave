using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.MediaItem;

public record CreateMediaItemDto(
	Guid id,
	string Name,
	string? OriginalName,
	string? Description,
	string? PosterPath,
	int? Duration,
	int? EpisodesCount,
	decimal? ImdbScore,
	int MediaItemTypeId,
	long? StatusId,
	long? RestrictedRatingId,
	DateTime? FirstAirDate,
	DateTime? LastAirDate,
	DateTime? PublishedAt,
	SeoAdditionDto SeoAddition
);