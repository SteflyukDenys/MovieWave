using Bogus;
using MovieWave.Domain.Entity;
namespace MovieWave.DAL.Seeders.DataGenerators;

public static class RestrictedRatingDataGenerator
{
	public static List<RestrictedRating> GenerateRestrictedRatings(int count)
	{
		return new Faker<RestrictedRating>()
			.RuleFor(rr => rr.Id, f => f.Random.Long())
			.RuleFor(rr => rr.Name, f => f.Lorem.Word())
			.RuleFor(rr => rr.Slug, f => f.Lorem.Slug())
			.RuleFor(rr => rr.Value, f => f.Random.Int(0, 18))
			.RuleFor(rr => rr.Hint, f => f.Lorem.Sentence())
			.Generate(count);
	}
}

