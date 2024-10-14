using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(u => u.Id);
		builder.Property(u => u.Id).ValueGeneratedNever();

		builder.Property(u => u.UserName)
			   .HasMaxLength(32)
			   .IsRequired();

		builder.Property(u => u.Email)
			   .HasMaxLength(255)
			   .IsRequired();
		builder.Property(u => u.NormalizedEmail)
			   .HasMaxLength(256);
		builder.Property(u => u.EmailConfirmed)
			   .IsRequired();

		//builder.Property(u => u.PasswordHash)
		//	   .HasMaxLength(255)
		//	   .IsRequired();
		//builder.Property(u => u.SecurityStamp)
		//	   .HasMaxLength(255)
		//	   .IsRequired();
		//builder.Property(u => u.ConcurrencyStamp)
		//	   .IsRequired()
		//	   .IsConcurrencyToken();

		//builder.Property(u => u.UserRole)
		//	   .IsRequired();

		builder.Property(u => u.Login)
			   .HasMaxLength(32)
			   .IsRequired();

		builder.Property(u => u.AvatarPath)
			   .HasMaxLength(255);
		builder.Property(u => u.BackdropPath)
			   .HasMaxLength(255);

		// Indexes
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
	}
}