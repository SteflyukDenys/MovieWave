using Bogus;
using MovieWave.Domain.Entity;
namespace MovieWave.DAL.Seeders.DataGenerators;

public static class StudioDataGenerator
{
	public static List<Studio> GenerateStudios(int count)
	{
		return new Faker<Studio>()
			.RuleFor(c => c.Id, f => f.Random.Guid())
			.RuleFor(s => s.Name, f => f.Company.CompanyName())
			.RuleFor(s => s.LogoPath, f => f.Image.PicsumUrl())
			.RuleFor(s => s.Description, f => f.Lorem.Paragraph())
			.RuleFor(s => s.SeoAddition, _ => SeoAdditionDataGenerator.GenerateSeoAddition())
			.Generate(count);
	}
}

