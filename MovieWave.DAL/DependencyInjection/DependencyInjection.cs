using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieWave.DAL.Repositories;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Repositories;

namespace MovieWave.DAL.DependencyInjection;

public static class DependencyInjection
{
	public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("SQLServer");

		services.AddDbContext<AppDbContext>(options =>
		{
			options.UseSqlServer(connectionString);
		});
		services.InitRepositories();
	}

	private static void InitRepositories(this IServiceCollection services)
	{
		//services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
	}
}
