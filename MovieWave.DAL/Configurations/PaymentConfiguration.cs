using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
	public void Configure(EntityTypeBuilder<Payment> builder)
	{
		builder.HasKey(p => p.Id);
		builder.Property(p => p.Id).ValueGeneratedNever();

		builder.HasOne(p => p.UserSubscription)
			.WithMany(u => u.Payments)
			.HasForeignKey(p => p.UserSubscriptionId);

		builder.Property(p => p.Amount)
			.HasColumnType("money")
			.IsRequired();

		builder.Property(p => p.PaymentDate).IsRequired();
		builder.Property(p => p.Status).IsRequired();
		builder.Property(p => p.BillingType).IsRequired();
	}
}
