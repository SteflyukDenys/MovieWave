using Bogus;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class UserSubscriptionDataGenerator
{
	public static List<UserSubscription> GenerateUserSubscriptions(int count, List<User> users, List<SubscriptionPlan> subscriptionPlans)
	{
		return new Faker<UserSubscription>()
			.RuleFor(us => us.Id, f => f.Random.Guid())
			.RuleFor(us => us.UserId, f => f.PickRandom(users).Id)
			.RuleFor(us => us.SubscriptionPlanId, f => f.PickRandom(subscriptionPlans).Id)
			.RuleFor(us => us.StartDate, f => f.Date.Past().ToUniversalTime())
			.RuleFor(us => us.EndDate, f => f.Date.Future().ToUniversalTime())
			.RuleFor(us => us.IsActive, f => f.Random.Bool())
			.Generate(count);
	}
}
