using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using MovieWave.DAL.Interceptors;
using MovieWave.Domain.Entity;
using System.Reflection;

namespace MovieWave.DAL;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(new DateInterceptor());
		optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasPostgresExtension("pg_trgm");
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public DbSet<Attachment> Attachments { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Country> Countries { get; set; }
	public DbSet<Episode> Episodes { get; set; }
	public DbSet<EpisodeVoice> EpisodeVoices { get; set; }
	public DbSet<MediaItem> MediaItems { get; set; }
	public DbSet<MediaItemPerson> MediaItemPeople { get; set; }
	public DbSet<MediaItemType> MediaItemTypes { get; set; }
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<Payment> Payments { get; set; }
	public DbSet<Person> People { get; set; }
	public DbSet<RestrictedRating> RestrictedRatings { get; set; }
	public DbSet<Review> Reviews { get; set; }
	public DbSet<Season> Seasons { get; set; }
	public DbSet<SeoAddition> SeoAdditions { get; set; }
	public DbSet<Status> Statuses { get; set; }
	public DbSet<Studio> Studios { get; set; }
	public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
	public DbSet<Tag> Tags { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<UserToken> UserToken { get; set; }
	public DbSet<UserMediaItemList> UserMediaItemLists { get; set; }
	public DbSet<UserSubscription> UserSubscriptions { get; set; }
	public DbSet<Voice> Voices { get; set; }
}
