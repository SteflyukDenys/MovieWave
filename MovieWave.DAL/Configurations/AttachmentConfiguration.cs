using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
	public void Configure(EntityTypeBuilder<Attachment> builder)
	{
		builder.HasKey(a => a.Id);
		builder.Property(a => a.Id).ValueGeneratedNever();

		builder.HasOne(a => a.MediaItem)
			.WithMany(m => m.Attachments)
			.HasForeignKey(a => a.MediaItemId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Property(a => a.AttachmentType).IsRequired();
		builder.Property(a => a.AttachmentUrl).IsRequired();
	}
}
