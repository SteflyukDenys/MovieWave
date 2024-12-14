using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class TmdbImportController : ControllerBase
	{
		private readonly ITmdbMovieImportService _tmdbMovieImportService;

		public TmdbImportController(ITmdbMovieImportService tmdbMovieImportService)
		{
			_tmdbMovieImportService = tmdbMovieImportService;
		}

		/// <summary>
		/// Запустити імпорт даних (300 фільмів та 50 серіалів) з TMDb
		/// </summary>
		/// <response code="200">Якщо імпорт успішно запущено</response>
		/// <response code="400">Якщо сталася помилка</response>
		[HttpPost("import")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> ImportData()
		{
			try
			{
				await _tmdbMovieImportService.ImportDataAsync();
				return Ok(new BaseResult { ErrorMessage = "Імпорт успішно завершено" });
			}
			catch (Exception ex)
			{
				return BadRequest(new BaseResult { ErrorMessage = ex.Message, ErrorCode = 400 });
			}
		}
	}
}