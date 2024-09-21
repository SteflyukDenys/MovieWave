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

		builder.HasOne(t => t.SeoAddition)
			.WithOne(s => s.MediaItemType)
			.HasForeignKey<MediaItemType>(t => t.SeoAdditionId);
	}
}
