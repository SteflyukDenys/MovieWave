using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
	public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
	{
		builder.HasKey(s => s.Name); // Name is [pk] (enum)
		builder.Property(s => s.Name)
			.HasColumnName("SubLevel");

		builder.Property(s => s.PricePerMonth)
			.HasColumnType("money")
			.IsRequired();

		builder.Property(s => s.MaxDevices).IsRequired();
		builder.Property(s => s.VideoQuality).IsRequired();
	}
}
