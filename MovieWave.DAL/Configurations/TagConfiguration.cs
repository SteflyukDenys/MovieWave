using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
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

		builder.HasOne(t => t.Parent)
			.WithMany(pt => pt.Children)
			.HasForeignKey(t => t.ParentId);
	}
}
