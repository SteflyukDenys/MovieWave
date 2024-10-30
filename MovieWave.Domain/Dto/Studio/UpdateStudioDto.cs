using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Studio;
public record UpdateStudioDto(long Id, string Name, string? LogoPath, string? Description, SeoAdditionDto SeoAddition);