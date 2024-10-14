using Bogus;
using MovieWave.Domain.Entity;
namespace MovieWave.DAL.Seeders.DataGenerators;

public static class EpisodeDataGenerator
{
	public static List<Episode> GenerateEpisodes(int count, List<MediaItem> mediaItems, List<Season> seasonItem)
	{
		var mediaItemIds = mediaItems.Select(mi => mi.Id).ToList();
		var seasonsItemIds = seasonItem.Select(si => si.Id).ToList();

		return new Faker<Episode>()
			.RuleFor(e => e.Id, f => f.Random.Guid())
			.RuleFor(m => m.Name, f => f.Lorem.Word())
			.RuleFor(e => e.MediaItemId, f => f.PickRandom(mediaItemIds))
			.RuleFor(e => e.SeasonId, f => f.PickRandom(seasonsItemIds))
			.RuleFor(e => e.Description, f => f.Lorem.Paragraph())
			.RuleFor(e => e.Duration, f => f.Random.Int(20, 60))
			.RuleFor(e => e.AirDate, f => f.Date.Past().ToUniversalTime())
			.RuleFor(e => e.IsFiller, f => f.Random.Bool())
			.RuleFor(e => e.ImagePath, f => f.Image.PicsumUrl())
			.RuleFor(e => e.SeoAddition, _ => SeoAdditionDataGenerator.GenerateSeoAddition())
			.Generate(count);
	}
}

