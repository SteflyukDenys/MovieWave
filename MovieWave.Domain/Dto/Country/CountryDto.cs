using MovieWave.Domain.Dto.SeoAddition;

namespace MovieWave.Domain.Dto.Country;

public class CountryDto
{
	public long Id { get; set; }

	public string Name { get; set; }

	public SeoAdditionDto SeoAddition { get; set; }
};