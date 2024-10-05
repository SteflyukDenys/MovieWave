namespace MovieWave.Domain.Dto.MediaItem;

public record MediaItemDto(
        Guid Id,
        string Name,
        string? OriginalName,
        string? Description,
        string? PosterPath,
        int? Duration,
        int? EpisodesCount,
        decimal? ImdbScore,
        string? PublishedAt //DateTime
    );
