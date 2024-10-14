using Microsoft.AspNetCore.Mvc;
using MovieWave.DAL.Seeders;

[ApiController]
[Route("api/[controller]")]
public class DataSeederController : ControllerBase
{
	private readonly DataSeederHelper _dataSeederHelper;

	public DataSeederController(DataSeederHelper dataSeederHelper)
	{
		_dataSeederHelper = dataSeederHelper;
	}

	[HttpPost]
	[Route("seed-database")]
	public async Task<IActionResult> SeedDatabase()
	{
		await _dataSeederHelper.SeedAsync();
		return Ok("Database seeded successfully");
	}
}
