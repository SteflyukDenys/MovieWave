using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using System.Security.Claims;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/ratings")]
	[Authorize]
	public class RatingsController : ControllerBase
	{
		private readonly IRatingsService _ratingsService;

		public RatingsController(IRatingsService ratingsService)
		{
			_ratingsService = ratingsService;
		}

		/// <summary>
		/// Оцінити медіаелемент
		/// </summary>
		/// <param name="dto">Об'єкт оцінки</param>
		/// <response code="200">Якщо оцінка успішно додана/оновлена</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BaseResult<bool>>> RateMediaItem([FromBody] RateMediaItemDto dto)
		{
			var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!Guid.TryParse(userIdClaim, out var userId))
			{
				return BadRequest(new BaseResult<bool>
				{
					ErrorMessage = "Некоректний userId.",
					ErrorCode = 400
				});
			}

			var result = await _ratingsService.RateMediaItemAsync(userId, dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		/// <summary>
		/// Отримати середню оцінку медіаелемента
		/// </summary>
		/// <param name="mediaItemId">ID медіаелемента</param>
		/// <response code="200">Середня оцінка</response>
		/// <response code="400">Якщо медіаелемент не знайдено</response>
		[HttpGet("media/{mediaItemId}")]
		[ProducesResponseType(typeof(BaseResult<double>), 200)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetAverageRating(Guid mediaItemId)
		{
			var result = await _ratingsService.GetAverageRatingAsync(mediaItemId);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}
	}
}
