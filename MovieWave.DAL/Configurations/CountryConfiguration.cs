using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations
{
	public class CountryConfiguration : IEntityTypeConfiguration<Country>
	{
		public void Configure(EntityTypeBuilder<Country> builder)
		{
			builder.HasKey(c => c.Id);
			builder.Property(c => c.Id).ValueGeneratedOnAdd();
			builder.Property(c => c.Name).IsRequired();

			// Composition
			builder.OwnsOne(c => c.SeoAddition, seo =>
			{
				seo.Property(s => s.Slug)
					.HasMaxLength(30)
					.IsRequired();

				seo.HasIndex(s => s.Slug).IsUnique();
			});

		}
	}
}
