using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class MediaItemPeopleDataGenerator
{
	public static List<MediaItemPerson> GenerateMediaItemPeople(int count, List<MediaItem> mediaItems, List<User> users)
	{
		return new Faker<MediaItemPerson>()
			.RuleFor(mip => mip.MediaItemId, f => f.PickRandom(mediaItems).Id)
			.RuleFor(mip => mip.PersonId, f => f.PickRandom(users).Id)
			.RuleFor(mip => mip.PersonRole, f => f.PickRandom<PersonRole>())
			.Generate(count);
	}
}