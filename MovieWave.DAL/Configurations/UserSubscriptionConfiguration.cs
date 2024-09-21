using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
{
	public void Configure(EntityTypeBuilder<UserSubscription> builder)
	{
		builder.HasKey(us => us.Id);
		builder.Property(us => us.Id).ValueGeneratedNever();

		builder.HasOne(us => us.User)
			.WithMany(u => u.UserSubscriptions)
			.HasForeignKey(us => us.UserId);

		builder.HasOne(us => us.SubscriptionPlan)
			.WithMany(s => s.UserSubscriptions)
			.HasForeignKey(us => us.SubscriptionPlanName);

		builder.Property(us => us.StartDate).IsRequired();
		builder.Property(us => us.EndDate).IsRequired();

		builder.Property(us => us.IsActive).IsRequired();

		// The user should have only one active subscription
		builder.HasIndex(us => new { us.UserId, us.IsActive })
			   .IsUnique();
	}
}
