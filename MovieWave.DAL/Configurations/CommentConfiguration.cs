using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations
{
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
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(c => c.Text).IsRequired();

			// Поля для зберігання ID і типу сутності, до якої прив'язаний коментар
			builder.Property(c => c.CommentableId)
				.IsRequired(false); // Поле може бути null, якщо коментар ще не прив'язано

			builder.Property(c => c.CommentableType)
				.HasMaxLength(50)
				.IsRequired(false); // Поле може бути null

			// Індексація полів для швидшого пошуку
			builder.HasIndex(c => new { c.CommentableId, c.CommentableType });
		}
	}
}
