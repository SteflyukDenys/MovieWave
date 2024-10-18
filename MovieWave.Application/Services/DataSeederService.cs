using MovieWave.Application.Resources;
using MovieWave.DAL.Seeders;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.Application.Services
{
	public class DataSeederService : IDataSeederService
	{
		private readonly DataSeederHelper _dataSeederHelper;

		public DataSeederService(DataSeederHelper dataSeederHelper)
		{
			_dataSeederHelper = dataSeederHelper;
		}

		public async Task<BaseResult<string>> SeedDatabaseAsync()
		{
			try
			{
				await _dataSeederHelper.SeedAsync();
				return new BaseResult<string>()
				{
					Data = "Database seeded successfully"
				};
			}
			catch (Exception ex)
			{
				return new BaseResult<string>()
				{
					ErrorMessage = ErrorMessage.ErrorSeedingDB,
					ErrorCode = (int)ErrorCodes.ErrorSeedingDb,
				};
			}
		}
	}
}