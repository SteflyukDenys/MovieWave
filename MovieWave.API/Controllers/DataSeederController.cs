using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DataSeederController : ControllerBase
{
	private readonly IDataSeederService _dataSeederService;

	public DataSeederController(IDataSeederService dataSeederService)
	{
		_dataSeederService = dataSeederService;
	}

	[HttpPost("db-seed")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> SeedDatabase()
	{
		var response = await _dataSeederService.SeedDatabaseAsync();

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}