using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Country;

public record CreateCountryDto(string Name, SeoAdditionDto SeoAddition);