using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
	}
}
