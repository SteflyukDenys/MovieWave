using Microsoft.EntityFrameworkCore;

namespace MovieWave.DAL;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
		Database.EnsureCreated();
	}
}
