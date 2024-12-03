using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieWave.DAL.Interceptors;
using MovieWave.DAL.Repositories;
using MovieWave.DAL.Seeders;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using System;
using MovieWave.Domain.Interfaces.Databases;

namespace MovieWave.DAL.DependencyInjection;

public static class DependencyInjection
{
	public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("PostgreSQL");
		services.AddSingleton<DateInterceptor>();
		services.AddDbContext<AppDbContext>(options =>
		{
			options.UseNpgsql(connectionString);
		});
		services.InitRepositories();
		services.AddScoped<DataSeederHelper>();
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
		services.AddScoped<IBaseRepository<Comment>, BaseRepository<Comment>>();
		services.AddScoped<IBaseRepository<Status>, BaseRepository<Status>>();
		services.AddScoped<IBaseRepository<Country>, BaseRepository<Country>>();
		services.AddScoped<IBaseRepository<Studio>, BaseRepository<Studio>>();
		services.AddScoped<IBaseRepository<Person>, BaseRepository<Person>>();
		services.AddScoped<IBaseRepository<EpisodeVoice>, BaseRepository<EpisodeVoice>>();
		services.AddScoped<IBaseRepository<Voice>, BaseRepository<Voice>>();
		services.AddScoped<IBaseRepository<MediaItemType>, BaseRepository<MediaItemType>>();
		services.AddScoped<IBaseRepository<RestrictedRating>, BaseRepository<RestrictedRating>>();
		services.AddScoped<IBaseRepository<Attachment>, BaseRepository<Attachment>>();
		services.AddScoped<IBaseRepository<Banner>, BaseRepository<Banner>>();

	// For User
	services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
		services.AddScoped<IBaseRepository<UserToken>, BaseRepository<UserToken>>();
		services.AddScoped<IBaseRepository<Role>, BaseRepository<Role>>();
		services.AddScoped<IBaseRepository<UserRole>, BaseRepository<UserRole>>();
		services.AddScoped<IBaseRepository<UserMediaItemList>, BaseRepository<UserMediaItemList>>();
		services.AddScoped<IBaseRepository<UserSubscription>, BaseRepository<UserSubscription>>();
	}
}
