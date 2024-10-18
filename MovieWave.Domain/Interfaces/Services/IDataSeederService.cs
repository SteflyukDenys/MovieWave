using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services
{
	public interface IDataSeederService
	{
		Task<BaseResult<string>> SeedDatabaseAsync();
	}
}