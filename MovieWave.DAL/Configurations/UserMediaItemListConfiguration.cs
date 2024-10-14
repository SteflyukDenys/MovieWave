using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations
{
	public class UserMediaItemListConfiguration : IEntityTypeConfiguration<UserMediaItemList>
	{
		public void Configure(EntityTypeBuilder<UserMediaItemList> builder)
		{
			builder.HasKey(umil => new { umil.UserId, umil.MediaItemId });

			builder.HasOne(umil => umil.User)
				.WithMany(u => u.UserMediaItemLists)
				.HasForeignKey(umil => umil.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(umil => umil.MediaItem)
				.WithMany(mi => mi.UserMediaItemLists)
				.HasForeignKey(umil => umil.MediaItemId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(umil => umil.ListTypeId)
				.IsRequired();
		}
	}
}
