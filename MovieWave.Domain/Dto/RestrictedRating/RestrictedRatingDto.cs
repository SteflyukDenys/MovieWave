namespace MovieWave.Domain.Dto.RestrictedRating;

public record RestrictedRatingDto(long Id, string Name, string Slug, int Value, string Hint);