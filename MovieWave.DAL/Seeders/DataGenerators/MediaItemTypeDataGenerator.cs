using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
namespace MovieWave.DAL.Seeders.DataGenerators;

public static class MediaItemTypeDataGenerator
{
	public static List<MediaItemType> GenerateMediaItemTypes(int count)
	{
		return new Faker<MediaItemType>()
			.RuleFor(t => t.Id, f => f.IndexFaker + 1) 
			.RuleFor(t => t.Name, f => f.PickRandom<MediaItemName>())
			.RuleFor(e => e.SeoAddition, _ => SeoAdditionDataGenerator.GenerateSeoAddition())
			.Generate(count);
	}
}
