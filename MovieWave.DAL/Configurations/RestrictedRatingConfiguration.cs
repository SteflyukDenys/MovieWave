using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class RestrictedRatingConfiguration : IEntityTypeConfiguration<RestrictedRating>
{
	public void Configure(EntityTypeBuilder<RestrictedRating> builder)
	{
		builder.HasKey(r => r.Id);
		builder.Property(r => r.Id).ValueGeneratedOnAdd();

		builder.Property(r => r.Name).IsRequired();
		builder.Property(r => r.Slug).IsRequired();
		builder.Property(r => r.Value).IsRequired();
		builder.Property(r => r.Hint).IsRequired();

	}
}
