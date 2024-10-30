using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;

namespace MovieWave.DAL.Configurations;

public class StatusConfiguration : IEntityTypeConfiguration<Status>
{
	public void Configure(EntityTypeBuilder<Status> builder)
	{
		builder.HasKey(s => s.Id);
		builder.Property(s => s.Id).ValueGeneratedOnAdd();

		builder.Property(s => s.StatusType).IsRequired();

		builder.HasMany(s => s.MediaItems)
			.WithOne(m => m.Status)
			.HasForeignKey(m => m.StatusId);

	}
}
