using Bogus;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class CommentDataGenerator
{
	public static List<Comment> GenerateComments(int count, List<Episode> episodes, List<MediaItem> mediaItems, List<Guid> userIds, List<Comment>? parentComments)
	{
		var episodeIds = episodes.Select(e => e.Id).ToList();
		var mediaItemIds = mediaItems.Select(m => m.Id).ToList();

		return new Faker<Comment>()
			.RuleFor(c => c.Id, f => f.Random.Guid())
			.RuleFor(c => c.CommentableId, f => f.PickRandom(episodeIds.Concat(mediaItemIds)))
			.RuleFor(c => c.CommentableType, f => f.PickRandom(new[] { "MediaItem", "Episode" }))
			.RuleFor(c => c.UserId, f => f.PickRandom(userIds))
			.RuleFor(c => c.ParentId, f => parentComments != null && parentComments.Count > 0 ? f.PickRandom(parentComments).Id : null)
			.RuleFor(c => c.Text, f => f.Lorem.Paragraph())
			.RuleFor(c => c.CreatedAt, f => f.Date.Past().ToUniversalTime())
			.RuleFor(c => c.UpdatedAt, f => f.Date.Past().ToUniversalTime())
			.Generate(count);
	}
}
