using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class UserMediaItemListDataGenerator
{
	public static List<UserMediaItemList> GenerateUserMediaItemLists(int count, List<User> users, List<MediaItem> mediaItems)
	{
		return new Faker<UserMediaItemList>()
			.RuleFor(umil => umil.UserId, f => f.PickRandom(users).Id)
			.RuleFor(umil => umil.MediaItemId, f => f.PickRandom(mediaItems).Id)
			.RuleFor(umil => umil.ListTypeId, f => f.PickRandom<ListType>())
			.Generate(count);
	}
}
