using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
	public void Configure(EntityTypeBuilder<Banner> builder)
	{
		builder.HasKey(b => b.Id);
		builder.Property(b => b.Id).ValueGeneratedNever();

		builder.Property(a => a.Title).HasMaxLength(255).IsRequired();
		builder.Property(a => a.ImageUrl).IsRequired();
	}
}