using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class Payment : BaseEntity<Guid>
{
	public Payment()
	{
		Id = Guid.NewGuid();
	}

	public Guid UserSubscriptionId { get; set; }
	public UserSubscription UserSubscription { get; set; }

	public decimal Amount { get; set; }
	public DateTime PaymentDate { get; set; }
	public PaymentStatus Status { get; set; }
	public BillingType BillingType { get; set; }
}