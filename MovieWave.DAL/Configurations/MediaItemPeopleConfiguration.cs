using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations
{
	public class MediaItemPeopleConfiguration : IEntityTypeConfiguration<MediaItemPerson>
	{
		public void Configure(EntityTypeBuilder<MediaItemPerson> builder)
		{
			builder.HasKey(mip => new { mip.MediaItemId, mip.PersonId });

			builder.HasOne(mip => mip.MediaItem)
				.WithMany(mi => mi.MediaItemPeople)
				.HasForeignKey(mip => mip.MediaItemId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(mip => mip.Person)
				.WithMany(p => p.MediaItemPeople)
				.HasForeignKey(mip => mip.PersonId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(mip => mip.PersonRole)
				.IsRequired();
		}
	}
}
