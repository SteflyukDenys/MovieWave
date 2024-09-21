using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
	public void Configure(EntityTypeBuilder<Country> builder)
	{
		builder.HasKey(c => c.Id);
		builder.Property(c => c.Id).ValueGeneratedNever();

		builder.Property(c => c.Name).IsRequired();

		builder.HasOne(c => c.SeoAddition)
			.WithOne(s => s.Country)
			.HasForeignKey<Country>(c => c.SeoAdditionId);
	}
}
