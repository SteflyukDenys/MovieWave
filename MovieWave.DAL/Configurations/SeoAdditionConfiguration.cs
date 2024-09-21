using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class SeoAdditionConfiguration : IEntityTypeConfiguration<SeoAddition>
{
	public void Configure(EntityTypeBuilder<SeoAddition> builder)
	{
		builder.HasKey(s => s.Id);
		builder.Property(s => s.Id).ValueGeneratedOnAdd();

		builder.Property(s => s.Slug).IsRequired();
	}
}
