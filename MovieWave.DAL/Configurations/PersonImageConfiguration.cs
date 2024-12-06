using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieWave.Domain.Entity;

public class PersonImageConfiguration : IEntityTypeConfiguration<PersonImage>
{
	public void Configure(EntityTypeBuilder<PersonImage> builder)
	{
		builder.HasKey(pi => pi.Id);
		builder.Property(pi => pi.Id).ValueGeneratedNever();

		builder.Property(pi => pi.ImagePath)
			.IsRequired();

		builder.HasOne(pi => pi.Person)
			.WithMany(p => p.Images)
			.HasForeignKey(pi => pi.PersonId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}