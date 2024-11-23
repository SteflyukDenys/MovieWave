using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.Property(x => x.Id).ValueGeneratedOnAdd();
		builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
	}
}
