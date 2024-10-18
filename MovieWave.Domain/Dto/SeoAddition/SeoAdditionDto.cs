namespace MovieWave.Domain.Dto.SeoAddition;

public record SeoAdditionDto(
    string Slug,
    string? MetaTitle,
    string? Description,
    string? MetaDescription,
    string? MetaImagePath
);