using Bogus;
namespace MovieWave.DAL.Seeders.DataGenerators;

public static class PersonDataGenerator
{
	public static List<MovieWave.Domain.Entity.Person> GeneratePeople(int count)
	{
		return new Faker<MovieWave.Domain.Entity.Person>()
			.RuleFor(c => c.Id, f => f.Random.Guid())
			.RuleFor(p => p.FirstName, f => f.Name.FirstName())
			.RuleFor(p => p.LastName, f => f.Name.LastName())
			.RuleFor(p => p.BirthDate, f => f.Date.Past().ToUniversalTime())
			.RuleFor(p => p.DeathDate, f => f.Date.Past().ToUniversalTime())
			.RuleFor(p => p.ImagePath, f => f.Image.PicsumUrl())
			.RuleFor(p => p.Biography, f => f.Lorem.Paragraph())
			.RuleFor(p => p.SeoAddition, _ => SeoAdditionDataGenerator.GenerateSeoAddition())
			.RuleFor(p => p.CreatedAt, f => f.Date.Past().ToUniversalTime())
			.RuleFor(p => p.UpdatedAt, f => f.Date.Past().ToUniversalTime())
			.Generate(count);
	}
}

