using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class UserDataGenerator
{
	public static List<User> GenerateUsers(int count)
	{
		return new Faker<User>()
			.RuleFor(u => u.Id, f => f.Random.Guid())
			.RuleFor(u => u.UserName, f => f.Internet.UserName())
			.RuleFor(u => u.Email, f => f.Internet.Email())
			.RuleFor(u => u.NormalizedEmail, (f, u) => u.Email.ToUpper())
			.RuleFor(u => u.EmailConfirmed, f => f.Random.Bool())
			.RuleFor(u => u.Login, f => f.Internet.UserName())
			.RuleFor(u => u.AvatarPath, f => f.Internet.Avatar())
			.RuleFor(u => u.CreatedAt, f => f.Date.Past().ToUniversalTime())
			.Generate(count);
	}
}
