using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;


public class MediaItemTypeConfiguration : IEntityTypeConfiguration<MediaItemType>
{
	public void Configure(EntityTypeBuilder<MediaItemType> builder)
	{
		builder.HasKey(t => t.Id);
		builder.Property(t => t.Id).ValueGeneratedOnAdd();

		builder.Property(t => t.Name).IsRequired();

		// Composition
		builder.OwnsOne(m => m.SeoAddition, seo =>
		{
			seo.Property(s => s.Slug)
			.HasMaxLength(30)
			.IsRequired();

			seo.HasIndex(s => s.Slug).IsUnique();
		});
	}
}
