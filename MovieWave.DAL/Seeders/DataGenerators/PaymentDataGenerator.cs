using Bogus;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
namespace MovieWave.DAL.Seeders.DataGenerators;

public static class PaymentDataGenerator
{
	public static List<Payment> GeneratePayments(int count)
	{
		return new Faker<Payment>()
			.RuleFor(c => c.Id, f => f.Random.Guid())
			.RuleFor(p => p.UserSubscriptionId, f => f.Random.Guid())
			.RuleFor(p => p.Amount, f => f.Finance.Amount())
			.RuleFor(p => p.PaymentDate, f => f.Date.Past().ToUniversalTime())
			.RuleFor(p => p.Status, f => f.PickRandom<PaymentStatus>())
			.RuleFor(p => p.BillingType, f => f.PickRandom<BillingType>())
			.Generate(count);
	}
}