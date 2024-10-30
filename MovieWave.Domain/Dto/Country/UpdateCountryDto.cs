using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Country;

public record UpdateCountryDto(long Id, string Name, SeoAdditionDto SeoAddition);