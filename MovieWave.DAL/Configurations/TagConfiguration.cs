using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
	public void Configure(EntityTypeBuilder<Tag> builder)
	{
		builder.HasKey(t => t.Id);
		builder.Property(t => t.Id).ValueGeneratedNever();

		builder.Property(t => t.Name).IsRequired();
		builder.Property(t => t.IsGenre).HasDefaultValue(false);

		// Configuring the self-referencing relationship
		builder.HasOne(t => t.Parent)
			.WithMany(pt => pt.Children)
			.HasForeignKey(t => t.ParentId)
			.OnDelete(DeleteBehavior.Restrict); // Avoid cascade deletion

		// Composition: Configuring SeoAddition
		builder.OwnsOne(t => t.SeoAddition, seo =>
		{
			seo.Property(s => s.Slug)
				.HasMaxLength(30)
				.IsRequired();

			seo.HasIndex(s => s.Slug).IsUnique();
		});

		// Many-to-Many relationship with MediaItem
		builder.HasMany(t => t.MediaItems)
			.WithMany(mi => mi.Tags);
	}
}
