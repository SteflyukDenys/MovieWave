using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using System.Security.Claims;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/user-media-items")]
	[Authorize]
	public class UserMediaItemsController : ControllerBase
	{
		private readonly ISaveMediaItemUsers _mediaItemUsersService;

		public UserMediaItemsController(ISaveMediaItemUsers mediaItemUsersService)
		{
			_mediaItemUsersService = mediaItemUsersService;
		}

		/// <summary>
		/// Отримати історію переглядів користувача
		/// </summary>
		/// <param name="pageNumber">Номер сторінки</param>
		/// <param name="pageSize">Розмір сторінки</param>
		/// <response code="200">Якщо історія успішно отримана</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpGet("watch-history")]
		[ProducesResponseType(typeof(BaseResult<List<MediaItemDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BaseResult<List<MediaItemDto>>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetWatchHistory([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			var userId = GetUserId();
			var result = await _mediaItemUsersService.GetWatchHistoryAsync(userId, pageNumber, pageSize);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		/// <summary>
		/// Додати медіаелемент до історії переглядів
		/// </summary>
		/// <param name="mediaItemId">ID медіаелемента</param>
		/// <response code="200">Якщо додано успішно</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpPost("watch-history/{mediaItemId}")]
		[ProducesResponseType(typeof(BaseResult<bool>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BaseResult<bool>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddToWatchHistory(Guid mediaItemId)
		{
			var userId = GetUserId();
			var result = await _mediaItemUsersService.AddToWatchHistoryAsync(userId, mediaItemId);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		/// <summary>
		/// Отримати обрані медіаелементи користувача
		/// </summary>
		/// <param name="pageNumber">Номер сторінки</param>
		/// <param name="pageSize">Розмір сторінки</param>
		/// <response code="200">Якщо обрані успішно отримані</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpGet("favorites")]
		[ProducesResponseType(typeof(BaseResult<List<MediaItemDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BaseResult<List<MediaItemDto>>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetFavorites([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			var userId = GetUserId();
			var result = await _mediaItemUsersService.GetFavoritesAsync(userId, pageNumber, pageSize);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		/// <summary>
		/// Додати медіаелемент до обраних
		/// </summary>
		/// <param name="mediaItemId">ID медіаелемента</param>
		/// <response code="200">Якщо додано успішно</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpPost("favorites/{mediaItemId}")]
		[ProducesResponseType(typeof(BaseResult<bool>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BaseResult<bool>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddToFavorites(Guid mediaItemId)
		{
			var userId = GetUserId();
			var result = await _mediaItemUsersService.AddToFavoritesAsync(userId, mediaItemId);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		/// <summary>
		/// Отримати список "Хочу подивитися" користувача
		/// </summary>
		/// <param name="pageNumber">Номер сторінки</param>
		/// <param name="pageSize">Розмір сторінки</param>
		/// <response code="200">Якщо список успішно отриманий</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpGet("want-to-watch")]
		[ProducesResponseType(typeof(BaseResult<List<MediaItemDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BaseResult<List<MediaItemDto>>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetWantToWatch([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			var userId = GetUserId();
			var result = await _mediaItemUsersService.GetWantToWatchAsync(userId, pageNumber, pageSize);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		/// <summary>
		/// Додати медіаелемент до списку "Хочу подивитися"
		/// </summary>
		/// <param name="mediaItemId">ID медіаелемента</param>
		/// <response code="200">Якщо додано успішно</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpPost("want-to-watch/{mediaItemId}")]
		[ProducesResponseType(typeof(BaseResult<bool>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BaseResult<bool>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> AddToWantToWatch(Guid mediaItemId)
		{
			var userId = GetUserId();
			var result = await _mediaItemUsersService.AddToWantToWatchAsync(userId, mediaItemId);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		/// <summary>
		/// Отримати ID поточного користувача з токена
		/// </summary>
		/// <returns>ID користувача</returns>
		private Guid GetUserId()
		{
			var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (Guid.TryParse(userIdClaim, out var userId))
			{
				return userId;
			}

			throw new Exception("Invalid user ID in token.");
		}
	}
}
