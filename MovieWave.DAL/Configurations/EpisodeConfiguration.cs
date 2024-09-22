using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
	public void Configure(EntityTypeBuilder<Episode> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e => e.Id).ValueGeneratedNever();

		builder.HasOne(e => e.MediaItem)
			.WithMany(m => m.Episodes)
			.HasForeignKey(e => e.MediaItemId);

		builder.HasOne(e => e.Season)
			.WithMany(s => s.Episodes)
			.HasForeignKey(e => e.SeasonId);

		builder.Property(e => e.IsFiller).HasDefaultValue(false);
	}
}
