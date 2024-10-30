using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class VoiceConfiguration : IEntityTypeConfiguration<Voice>
{
	public void Configure(EntityTypeBuilder<Voice> builder)
	{
		builder.HasKey(v => v.Id);
		builder.Property(v => v.Id).ValueGeneratedOnAdd();

		builder.Property(v => v.Name).IsRequired();

		builder.Property(v => v.Locale).IsRequired();
	}
}
