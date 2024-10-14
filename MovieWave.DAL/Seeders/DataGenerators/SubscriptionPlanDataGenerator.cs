using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using System.Collections.Generic;

namespace MovieWave.DAL.Seeders.DataGenerators
{
	public static class SubscriptionPlanDataGenerator
	{
		public static List<SubscriptionPlan> GenerateSubscriptionPlans(int count)
		{
			return new Faker<SubscriptionPlan>()
				.RuleFor(sp => sp.Name, f => f.PickRandom<SubscriptionLevel>())
				.RuleFor(sp => sp.Description, f => f.Lorem.Sentence())
				.RuleFor(sp => sp.PricePerMonth, f => f.Random.Decimal(5, 20))
				.RuleFor(sp => sp.MaxDevices, f => f.Random.Int(1, 5))
				.RuleFor(sp => sp.VideoQuality, f => f.PickRandom<VideoQuality>())
				.Generate(count);
		}
	}
}