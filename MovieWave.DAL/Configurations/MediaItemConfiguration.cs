using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class MediaItemConfiguration : IEntityTypeConfiguration<MediaItem>
{
	public void Configure(EntityTypeBuilder<MediaItem> builder)
	{
		builder.HasKey(m => m.Id);

		builder.Property(m => m.Id)
			.ValueGeneratedNever();

		builder.Property(m => m.Name)
			.HasMaxLength(255)
			.IsRequired();

		builder.HasOne(m => m.MediaItemType)
			.WithMany(t => t.MediaItems)
			.HasForeignKey(m => m.MediaItemTypeId);

		builder.Property(m => m.OriginalName)
			.HasMaxLength(255);

		builder.HasOne(m => m.Status)
			.WithMany(s => s.MediaItems)
			.HasForeignKey(m => m.StatusId);

		builder.HasOne(m => m.RestrictedRating)
			.WithMany(r => r.MediaItems)
			.HasForeignKey(m => m.RestrictedRatingId);

		builder.Property(m => m.PosterPath)
			.HasMaxLength(255);

		builder.Property(m => m.ImdbScore)
			.HasColumnType("decimal(3, 1)");

		builder.HasOne(m => m.SeoAddition)
		   .WithOne(s => s.MediaItem)
		   .HasForeignKey<MediaItem>(m => m.SeoAdditionId);

		// Many-to-Many 
		builder.HasMany(m => m.Countries)
		.WithMany(c => c.MediaItems)
		.UsingEntity<Dictionary<string, object>>(
			"MediaItemCountries",
			l => l.HasOne<Country>().WithMany().HasForeignKey("CountryId"),
			l => l.HasOne<MediaItem>().WithMany().HasForeignKey("MediaItemId"));

		builder.HasMany(m => m.Studios)
			.WithMany(s => s.MediaItems)
			.UsingEntity<Dictionary<string, object>>(
				"MediaItemStudios",
				l => l.HasOne<Studio>().WithMany().HasForeignKey("StudioId"),
				l => l.HasOne<MediaItem>().WithMany().HasForeignKey("MediaItemId"));

		builder.HasMany(m => m.Tags)
			.WithMany(t => t.MediaItems)
			.UsingEntity<Dictionary<string, object>>(
				"MediaItemTags",
				l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
				l => l.HasOne<MediaItem>().WithMany().HasForeignKey("MediaItemId"));

		builder.HasMany(m => m.People)
			.WithMany(p => p.MediaItems)
			.UsingEntity<MediaItemPeople>(
				l => l.HasOne<Person>().WithMany().HasForeignKey(x => x.PersonId),
				l => l.HasOne<MediaItem>().WithMany().HasForeignKey(x => x.MediaItemId));
	}
}
