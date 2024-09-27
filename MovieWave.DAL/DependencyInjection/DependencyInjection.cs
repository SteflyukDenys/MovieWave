using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieWave.DAL.Interceptors;
using MovieWave.DAL.Repositories;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Repositories;

namespace MovieWave.DAL.DependencyInjection;

public static class DependencyInjection
{
	public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("SQLServer");

		services.AddSingleton<DateInterceptor>();
		services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlServer(connectionString);
		});
		services.InitRepositories();
	}

	private static void InitRepositories(this IServiceCollection services)
	{
		services.AddScoped<IBaseRepository<MediaItem>, BaseRepository<MediaItem>>();
		services.AddScoped<IBaseRepository<Episode>, BaseRepository<Episode>>();
		services.AddScoped<IBaseRepository<Season>, BaseRepository<Season>>();
		services.AddScoped<IBaseRepository<Tag>, BaseRepository<Tag>>();
		services.AddScoped<IBaseRepository<Review>, BaseRepository<Review>>();
		services.AddScoped<IBaseRepository<Notification>, BaseRepository<Notification>>();
		services.AddScoped<IBaseRepository<SubscriptionPlan>, BaseRepository<SubscriptionPlan>>();
		services.AddScoped<IBaseRepository<Payment>, BaseRepository<Payment>>();
		services.AddScoped<IBaseRepository<UserSubscription>, BaseRepository<UserSubscription>>();
		services.AddScoped<IBaseRepository<Comment>, BaseRepository<Comment>>();
		services.AddScoped<IBaseRepository<Status>, BaseRepository<Status>>();
		services.AddScoped<IBaseRepository<SeoAddition>, BaseRepository<SeoAddition>>();
		services.AddScoped<IBaseRepository<Country>, BaseRepository<Country>>();
		services.AddScoped<IBaseRepository<Studio>, BaseRepository<Studio>>();
		services.AddScoped<IBaseRepository<Person>, BaseRepository<Person>>();
		services.AddScoped<IBaseRepository<UserMediaItemList>, BaseRepository<UserMediaItemList>>();
		services.AddScoped<IBaseRepository<EpisodeVoice>, BaseRepository<EpisodeVoice>>();
		services.AddScoped<IBaseRepository<Voice>, BaseRepository<Voice>>();
		services.AddScoped<IBaseRepository<MediaItemType>, BaseRepository<MediaItemType>>();
		services.AddScoped<IBaseRepository<RestrictedRating>, BaseRepository<RestrictedRating>>();
		services.AddScoped<IBaseRepository<Attachment>, BaseRepository<Attachment>>();

		// For Identity
		services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
		services.AddScoped<IBaseRepository<IdentityRole<Guid>>, BaseRepository<IdentityRole<Guid>>>();
		services.AddScoped<IBaseRepository<IdentityUserRole<Guid>>, BaseRepository<IdentityUserRole<Guid>>>();
		services.AddScoped<IBaseRepository<IdentityUserToken<Guid>>, BaseRepository<IdentityUserToken<Guid>>>();
	}
}
