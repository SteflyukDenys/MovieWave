using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Interfaces.Services;

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

	/// <summary>
	/// Запуск процесу заповнення бази даних початковими даними
	/// </summary>
	/// <remarks>
	/// Приклад запиту:
	/// <code>
	/// POST /api/v1/dataseeder/db-seed
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо дані успішно додані в базу</response>
	/// <response code="400">Якщо сталася помилка при додаванні даних</response>
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