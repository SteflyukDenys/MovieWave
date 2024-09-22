using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MovieWave.Domain.Entity;

namespace MovieWave.DAL.Configurations;

public class SeoAdditionConfiguration : IEntityTypeConfiguration<SeoAddition>
{
	public void Configure(EntityTypeBuilder<SeoAddition> builder)
	{
		builder.HasKey(sa => sa.Id);
		builder.Property(sa => sa.Id).ValueGeneratedOnAdd();

		builder.Property(sa => sa.Slug).IsRequired();

		// Polymorphic Relationship
		builder.HasDiscriminator<string>("SeoableType")
			.HasValue<MediaItem>("MediaItem")
			.HasValue<MediaItemType>("MediaItemType")
			.HasValue<RestrictedRating>("RestrictedRating")
			.HasValue<Episode>("Episode")
			.HasValue<Voice>("Voice")
			.HasValue<Country>("Country")
			.HasValue<Studio>("Studio")
			.HasValue<Person>("Person")
			.HasValue<Notification>("Notification")
			.HasValue<Tag>("Tag")
			.HasValue<Status>("Status");

		builder.HasOne<MediaItem>()
			.WithOne(m => m.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<MediaItem>(m => m.Id);

		builder.HasOne<MediaItemType>()
			.WithOne(mt => mt.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<MediaItemType>(mt => mt.Id);

		builder.HasOne<RestrictedRating>()
			.WithOne(rr => rr.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<RestrictedRating>(rr => rr.Id);

		builder.HasOne<Episode>()
			.WithOne(e => e.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<Episode>(e => e.Id);

		builder.HasOne<Voice>()
			.WithOne(v => v.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<Voice>(v => v.Id);

		builder.HasOne<Country>()
			.WithOne()
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<Country>(c => c.Id);

		builder.HasOne<Studio>()
			.WithOne(s => s.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<Studio>(s => s.Id);

		builder.HasOne<Person>()
			.WithOne(p => p.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<Person>(p => p.Id);

		builder.HasOne<Notification>()
			.WithOne(n => n.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<Notification>(n => n.Id);

		builder.HasOne<Tag>()
			.WithOne(t => t.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<Tag>(t => t.Id);

		builder.HasOne<Status>()
			.WithOne(s => s.SeoAddition)
			.HasForeignKey<SeoAddition>(sa => sa.SeoableId)
			.HasPrincipalKey<Status>(s => s.Id);
	}
}