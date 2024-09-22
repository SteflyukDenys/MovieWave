using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
	public void Configure(EntityTypeBuilder<Season> builder)
	{
		builder.HasKey(s => s.Id);
		builder.Property(s => s.Id).ValueGeneratedNever();

		builder.Property(s => s.Name).IsRequired();

		builder.HasOne(s => s.MediaItem)
			.WithMany(m => m.Seasons)
			.HasForeignKey(s => s.MediaItemId);
	}
}
