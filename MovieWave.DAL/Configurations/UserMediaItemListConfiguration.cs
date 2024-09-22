using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class UserMediaItemListConfiguration : IEntityTypeConfiguration<UserMediaItemList>
{
	public void Configure(EntityTypeBuilder<UserMediaItemList> builder)
	{
		builder.HasKey(i => new { i.UserId, i.MediaItemId });

		builder.HasOne(i => i.User)
			   .WithMany(u => u.UserMediaItemLists)
			   .HasForeignKey(i => i.UserId);

		builder.HasOne(i => i.MediaItem)
			   .WithMany(m => m.UserMediaItemLists)
			   .HasForeignKey(i => i.MediaItemId);

		builder.Property(i => i.ListTypeId).IsRequired();
	}
}
