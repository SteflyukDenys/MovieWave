using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Studio;

public record StudioDto(long Id, string Name, string? LogoPath, string? Description, SeoAdditionDto SeoAddition);