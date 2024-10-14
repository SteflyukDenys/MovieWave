﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
	public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
	{
		builder.HasKey(sp => sp.Id);
		builder.Property(sp => sp.Id).ValueGeneratedOnAdd();

		builder.Property(sp => sp.Name)
			.HasColumnName("SubLevel")
			.IsRequired();

		builder.Property(sp => sp.PricePerMonth)
			.HasColumnType("money")
			.IsRequired();

		builder.Property(sp => sp.MaxDevices).IsRequired();
		builder.Property(sp => sp.VideoQuality).IsRequired();
	}
}
