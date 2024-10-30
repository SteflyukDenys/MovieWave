using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations
{
	public class StudioConfiguration : IEntityTypeConfiguration<Studio>
	{
		public void Configure(EntityTypeBuilder<Studio> builder)
		{
			builder.HasKey(s => s.Id);
			builder.Property(s => s.Id).ValueGeneratedOnAdd();
			builder.Property(s => s.Name).IsRequired();

			// Composition for SEO addition
			builder.OwnsOne(s => s.SeoAddition, seo =>
			{
				seo.Property(s => s.Slug)
				   .HasMaxLength(30)
				   .IsRequired();
				seo.Property(s => s.MetaTitle).HasMaxLength(60);
				seo.Property(s => s.Description).HasMaxLength(160);
				seo.Property(s => s.MetaDescription).HasMaxLength(160);
				seo.Property(s => s.MetaImagePath).HasMaxLength(255);

				seo.HasIndex(s => s.Slug).IsUnique();
			});
		}
	}
}
