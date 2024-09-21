using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class EpisodeVoiceConfiguration : IEntityTypeConfiguration<EpisodeVoice>
{
	public void Configure(EntityTypeBuilder<EpisodeVoice> builder)
	{
		builder.HasKey(ev => new { ev.EpisodeId, ev.VoiceId }); // Composite primary key

		builder.HasOne(ev => ev.Episode)
			.WithMany(e => e.EpisodeVoices)
			.HasForeignKey(ev => ev.EpisodeId);

		builder.HasOne(ev => ev.Voice)
			.WithMany(v => v.EpisodeVoices)
			.HasForeignKey(ev => ev.VoiceId);

		builder.Property(ev => ev.VideoUrl).IsRequired();
	}
}
