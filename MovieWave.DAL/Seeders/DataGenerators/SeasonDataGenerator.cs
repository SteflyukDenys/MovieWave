using Bogus;
using MovieWave.Domain.Entity;
namespace MovieWave.DAL.Seeders.DataGenerators;

public static class SeasonDataGenerator
{
	public static List<Season> GenerateSeasons(int count, List<MediaItem> mediaItems)
	{
		var mediaItemIds = mediaItems.Select(mi => mi.Id).ToList();
		return new Faker<Season>()
			.RuleFor(s => s.Id, f => f.Random.Guid()) // Унікальні ідентифікатори
			.RuleFor(s => s.MediaItemId, f => f.PickRandom(mediaItemIds))
			.RuleFor(s => s.Name, f => f.Lorem.Word())
			.Generate(count);
	}
}

