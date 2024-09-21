using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
	public void Configure(EntityTypeBuilder<Notification> builder)

	{
		builder.HasKey(n => n.Id);
		builder.Property(n => n.Id).ValueGeneratedNever();


		builder.HasOne(n => n.User)
			.WithMany(u => u.Notifications)
			.HasForeignKey(n => n.UserId);

		builder.HasOne(n => n.MediaItem)
			.WithMany(m => m.Notifications)
			.HasForeignKey(n => n.MediaItemId);

		builder.HasOne(n => n.Episode)
			.WithMany(e => e.Notifications)
			.HasForeignKey(n => n.EpisodeId);

		builder.Property(n => n.NotificationType).IsRequired();
		builder.Property(n => n.IsRead).HasDefaultValue(false);

		builder.HasOne(n => n.SeoAddition)
			.WithOne(s => s.Notification)
			.HasForeignKey<Notification>(n => n.SeoAdditionId);
	}
}
