using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
	public void Configure(EntityTypeBuilder<UserToken> builder)
	{
		builder.Property(x => x.Id).ValueGeneratedNever();
		builder.Property(x => x.RefreshToken).IsRequired();
		builder.Property(x => x.RefreshTokenExpireTime).IsRequired();

	}
}