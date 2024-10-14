using Bogus;
using MovieWave.Domain.Entity;
namespace MovieWave.DAL.Seeders.DataGenerators;

public static class CountryDataGenerator
{
	public static List<Country> GenerateCountries(int count)
	{
		return new Faker<Country>()
			.RuleFor(c => c.Id, f => f.Random.Guid())
			.RuleFor(c => c.Name, f => f.Address.Country())
			.RuleFor(c => c.SeoAddition, _ => SeoAdditionDataGenerator.GenerateSeoAddition())
			.Generate(count);
	}
}

