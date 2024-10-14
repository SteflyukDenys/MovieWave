using Bogus;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class ReviewDataGenerator
{
	public static List<Review> GenerateReviews(int count, List<MediaItem> mediaItems, List<User> users)
	{
		return new Faker<Review>()
			.RuleFor(r => r.Id, f => f.Random.Guid())
			.RuleFor(r => r.MediaItemId, f => f.PickRandom(mediaItems).Id)
			.RuleFor(r => r.UserId, f => f.PickRandom(users).Id)
			.RuleFor(r => r.Rating, f => f.Random.Int(1, 10))
			.RuleFor(r => r.Text, f => f.Lorem.Paragraph())
			.RuleFor(r => r.CreatedAt, f => f.Date.Past().ToUniversalTime())
			.RuleFor(r => r.UpdatedAt, f => f.Date.Past().ToUniversalTime())
			.Generate(count);
	}
}
