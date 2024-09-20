using MovieWave.Domain.AbstractEntity;
using MovieWave.Domain.Enum;

namespace MovieWave.Domain.Entity;

public class UserSubscription : BaseEntity<Guid>
{
	public Guid UserId { get; set; }
	public User User { get; set; }

	public SubscriptionLevel SubscriptionPlanName { get; set; }
	public SubscriptionPlan SubscriptionPlan { get; set; }

	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }

	public bool IsActive { get; set; }

	// One-to-Many
	public ICollection<Payment> Payments { get; set; }
}

