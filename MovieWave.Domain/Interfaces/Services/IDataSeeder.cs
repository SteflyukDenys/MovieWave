using System.Threading.Tasks;

namespace MovieWave.Domain.Interfaces.Services
{
	public interface IDataSeeder
	{
		Task SeedAsync();
	}
}
