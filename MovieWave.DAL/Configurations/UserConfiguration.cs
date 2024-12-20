﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;
using System.Reflection.Emit;

namespace MovieWave.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(u => u.Login)
			.HasMaxLength(32)
			.IsRequired();

		builder.Property(u => u.AvatarPath)
			.HasMaxLength(255);

		builder.HasIndex(u => u.NormalizedEmail).IsUnique();
		builder.HasIndex(u => u.Login).IsUnique();

		builder.HasMany(u => u.UserMediaItemLists)
			.WithOne(i => i.User)
			.HasForeignKey(i => i.UserId);

		builder.HasMany(u => u.Comments)
			.WithOne(c => c.User)
			.HasForeignKey(c => c.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(u => u.UserSubscriptions)
			.WithOne(us => us.User)
			.HasForeignKey(us => us.UserId);

		builder.HasMany(u => u.Notifications)
			.WithOne(n => n.User)
			.HasForeignKey(n => n.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(u => u.Reviews)
			.WithOne(r => r.User)
			.HasForeignKey(r => r.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(u => u.UserToken)
			.WithOne(ut => ut.User)
			.HasForeignKey<UserToken>(ut => ut.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(x => x.Roles)
			.WithMany(x => x.Users)
			.UsingEntity<UserRole>(
				l => l.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId),
				l => l.HasOne<User>().WithMany().HasForeignKey(x => x.UserId)
			);

	}
}