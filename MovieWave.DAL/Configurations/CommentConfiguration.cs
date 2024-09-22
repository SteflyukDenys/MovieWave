using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
	public void Configure(EntityTypeBuilder<Comment> builder)
	{
		builder.HasKey(c => c.Id);
		builder.Property(c => c.Id).ValueGeneratedNever();

		builder.HasOne(c => c.User)
			.WithMany(u => u.Comments)
			.HasForeignKey(c => c.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(c => c.Parent)
			.WithMany(pc => pc.Children)
			.HasForeignKey(c => c.ParentId)
			.OnDelete(DeleteBehavior.Restrict); // Prohibit parent comment deletion if child comments exist"

		builder.Property(c => c.Text).IsRequired();

		// Polymorphic Relationship
		builder.HasDiscriminator<string>("CommentableType")
		   .HasValue<MediaItem>("MediaItem")
		   .HasValue<Episode>("Episode");

		builder.HasOne<MediaItem>()
			.WithMany(m => m.Comments)
			.HasForeignKey(c => c.CommentableId)
			.HasPrincipalKey(m => m.Id)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne<Episode>()
			.WithMany(e => e.Comments)
			.HasForeignKey(c => c.CommentableId)
			.HasPrincipalKey(e => e.Id)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
