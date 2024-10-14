using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class AttachmentDataGenerator
{
	public static List<Attachment> GenerateAttachments(int count, List<MediaItem> mediaItems)
	{
		if (mediaItems == null || !mediaItems.Any())
		{
			throw new ArgumentException("The mediaItems list cannot be empty.", nameof(mediaItems));
		}

		var mediaItemIds = mediaItems.Select(mi => mi.Id).ToList();

		return new Faker<Attachment>()
			.RuleFor(a => a.Id, f => f.Random.Guid())
			.RuleFor(a => a.MediaItemId, f => f.PickRandom(mediaItemIds))
			.RuleFor(a => a.AttachmentType, f => f.PickRandom<AttachmentType>())
			.RuleFor(a => a.AttachmentUrl, f => f.Internet.Url())
			.RuleFor(a => a.ThumbnailPath, f => f.Image.PicsumUrl())
			.Generate(count);
	}
}
