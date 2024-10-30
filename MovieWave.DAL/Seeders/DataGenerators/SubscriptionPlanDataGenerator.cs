using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Seeders.DataGenerators;

public static class SubscriptionPlanDataGenerator
{
	public static List<SubscriptionPlan> GenerateSubscriptionPlans()
	{
		return new List<SubscriptionPlan>
		{
			new SubscriptionPlan
			{
				Id = Guid.NewGuid(),
				Name = SubscriptionLevel.Basic,
				Description = "Базовий план для одного пристрою з HD якістю.",
				PricePerMonth = 149.99m,
				MaxDevices = 1,
				VideoQuality = VideoQuality.HD
			},
			new SubscriptionPlan
			{
				Id = Guid.NewGuid(),
				Name = SubscriptionLevel.Standart,
				Description = "Стандартний план для двох пристроїв з Full HD якістю.",
				PricePerMonth = 249.99m,
				MaxDevices = 2,
				VideoQuality = VideoQuality.FullHD
			},
			new SubscriptionPlan
			{
				Id = Guid.NewGuid(),
				Name = SubscriptionLevel.Premium,
				Description = "Преміум план для чотирьох пристроїв з Ultra HD якістю.",
				PricePerMonth = 349.99m,
				MaxDevices = 4,
				VideoQuality = VideoQuality.UHD
			}
		};
	}
}