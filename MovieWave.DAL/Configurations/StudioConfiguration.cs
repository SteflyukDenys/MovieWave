using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class StudioConfiguration : IEntityTypeConfiguration<Studio>
{
	public void Configure(EntityTypeBuilder<Studio> builder)
	{
		builder.HasKey(s => s.Id);
		builder.Property(s => s.Id).ValueGeneratedNever();

		builder.Property(s => s.Name).IsRequired();
	}
}
