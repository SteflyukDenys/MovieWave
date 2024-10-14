using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class NotificationDataGenerator
{
	public static List<Notification> GenerateNotifications(int count, List<User> users, List<MediaItem> mediaItems, List<Episode> episodes)
	{
		var episodeIds = episodes.Select(e => e.Id).ToList();

		return new Faker<Notification>()
			.RuleFor(n => n.Id, f => f.Random.Guid())
			.RuleFor(n => n.UserId, f => f.PickRandom(users).Id)
			.RuleFor(n => n.MediaItemId, f => f.PickRandom(mediaItems).Id)
			.RuleFor(n => n.EpisodeId, f => f.PickRandom(episodeIds)) // Використовуємо наявні Id епізодів
			.RuleFor(n => n.NotificationType, f => f.PickRandom<NotificationType>())
			.RuleFor(n => n.IsRead, f => f.Random.Bool())
			.RuleFor(n => n.CreatedAt, f => f.Date.Past().ToUniversalTime())
			.RuleFor(n => n.UpdatedAt, f => f.Date.Past().ToUniversalTime())
			.Generate(count);
	}
}