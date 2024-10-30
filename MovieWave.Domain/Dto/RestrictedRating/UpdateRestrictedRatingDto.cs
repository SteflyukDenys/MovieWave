namespace MovieWave.Domain.Dto.RestrictedRating;

public record UpdateRestrictedRatingDto(long Id, string Name, string Slug, int Value, string Hint);