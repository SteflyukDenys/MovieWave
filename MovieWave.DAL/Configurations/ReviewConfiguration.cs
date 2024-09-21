using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.HasKey(r => r.Id);
		builder.Property(r => r.Id).ValueGeneratedNever();

		builder.HasOne(r => r.MediaItem)
			.WithMany(m => m.Reviews)
			.HasForeignKey(r => r.MediaItemId);

		builder.HasOne(r => r.User)
			.WithMany(u => u.Reviews)
			.HasForeignKey(r => r.UserId);

		builder.Property(r => r.Text).IsRequired();
	}
}
