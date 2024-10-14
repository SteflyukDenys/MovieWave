using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations
{
	public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
	{
		public void Configure(EntityTypeBuilder<UserSubscription> builder)
		{
			builder.HasKey(us => us.Id);

			builder.Property(us => us.StartDate).IsRequired();
			builder.Property(us => us.EndDate).IsRequired();
			builder.Property(us => us.IsActive).IsRequired();

			builder.HasOne(us => us.User)
				.WithMany(u => u.UserSubscriptions)
				.HasForeignKey(us => us.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(us => us.SubscriptionPlan)
				.WithMany(sp => sp.UserSubscriptions)
				.HasForeignKey(us => us.SubscriptionPlanId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
