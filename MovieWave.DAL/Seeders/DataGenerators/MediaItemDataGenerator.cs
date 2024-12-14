using Bogus;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class MediaItemDataGenerator
{
	public static List<MediaItem> GenerateMediaItems(int count, List<Status> statuses, List<MediaItemType> mediaItemTypes, List<RestrictedRating> restrictedRatings)
	{
		if (!statuses.Any() || !mediaItemTypes.Any() || !restrictedRatings.Any())
		{
			throw new InvalidOperationException("One or more required lists for media item generation are empty.");
		}

		return new Faker<MediaItem>()
			.RuleFor(m => m.Id, f => f.Random.Guid())
			.RuleFor(m => m.Name, f => f.Lorem.Word())
			.RuleFor(m => m.OriginalName, f => f.Lorem.Word())
			.RuleFor(m => m.Description, f => f.Lorem.Paragraph())
			.RuleFor(m => m.MediaItemTypeId, f => f.PickRandom(mediaItemTypes).Id)
			.RuleFor(m => m.StatusId, f => f.PickRandom(statuses).Id)
			.RuleFor(m => m.RestrictedRatingId, f => f.PickRandom(restrictedRatings).Id)
			.RuleFor(m => m.Duration, f => f.Random.Int(60, 180))
			.RuleFor(m => m.FirstAirDate, f => f.Date.Past().ToUniversalTime())
			.RuleFor(m => m.LastAirDate, f => f.Date.Past().ToUniversalTime())
			.RuleFor(m => m.ImdbScore, f => f.Random.Double(1, 10))
			.RuleFor(m => m.SeoAddition, _ => SeoAdditionDataGenerator.GenerateSeoAddition())
			.Generate(count);
	}
}
