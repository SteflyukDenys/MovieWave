using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
namespace MovieWave.DAL.Seeders.DataGenerators;

public static class StatusDataGenerator
{
	public static List<Status> GenerateStatuses(int count)
	{
		return new Faker<Status>()
			.RuleFor(s => s.Id, f => f.Random.Long())
			.RuleFor(s => s.StatusType, f => f.PickRandom<StatusType>())
			.RuleFor(s => s.Description, f => f.Lorem.Paragraph())
			.Generate(count);
	}
}