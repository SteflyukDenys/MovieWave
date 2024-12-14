using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Application.Validations.FluentValidations.MediaItem;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MediaItemController : ControllerBase
{
	private readonly IMediaItemService _mediaItemService;

	public MediaItemController(IMediaItemService mediaItemService)
	{
		_mediaItemService = mediaItemService;
	}

	/// <summary>
	/// Отримати всі фільми/серіали
	/// </summary>
	/// <response code="200">Якщо список медіаелементів успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpGet("all/")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> GetMediaItemAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var response = await _mediaItemService.GetMediaItemsAsync(pageNumber, pageSize);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Отримати фільм/серіал за ID
	/// </summary>
	/// <param name="id">Ідентифікатор медіаелемента</param>
	/// <response code="200">Якщо медіаелемент був знайдений</response>
	/// <response code="400">Якщо медіаелемент не знайдено або сталася помилка при запиті</response>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> GetMediaItemById(Guid id)
	{
		var response = await _mediaItemService.GetMediaItemByIdAsync(id);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
	/// <summary>
	/// Отримує список всіх фільмів з підтримкою пагінації.
	/// </summary>
	/// <param name="pageNumber">Номер сторінки (за замовчуванням: 1).</param>
	/// <param name="pageSize">Кількість елементів на сторінці (за замовчуванням: 10).</param>
	/// <returns>Список фільмів.</returns>
	[HttpGet("movies")]
	public async Task<IActionResult> GetAllMovies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		try
		{
			var result = await _mediaItemService.GetAllMoviesAsync(pageNumber, pageSize);

			if (result.IsSuccess && result.Data != null && result.Data.Any())
			{
				return Ok(new
				{
					success = true,
					data = result.Data,
					count = result.Count
				});
			}
			else
			{
				return NotFound(new
				{
					success = false,
					message = result.ErrorMessage ?? "Фільми не знайдено."
				});
			}
		}
		catch (Exception ex)
		{
			return StatusCode(500, new
			{
				success = false,
				message = "Внутрішня помилка сервера."
			});
		}
	}

	/// <summary>
	/// Отримує список всіх серіалів з підтримкою пагінації.
	/// </summary>
	/// <param name="pageNumber">Номер сторінки (за замовчуванням: 1).</param>
	/// <param name="pageSize">Кількість елементів на сторінці (за замовчуванням: 10).</param>
	/// <returns>Список серіалів.</returns>
	[HttpGet("series")]
	public async Task<IActionResult> GetAllSeries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		try
		{
			var result = await _mediaItemService.GetAllSeriesAsync(pageNumber, pageSize);

			if (result.IsSuccess && result.Data != null && result.Data.Any())
			{
				return Ok(new
				{
					success = true,
					data = result.Data,
					count = result.Count
				});
			}
			else
			{
				return NotFound(new
				{
					success = false,
					message = result.ErrorMessage ?? "Серіали не знайдено."
				});
			}
		}
		catch (Exception ex)
		{
			return StatusCode(500, new
			{
				success = false,
				message = "Внутрішня помилка сервера."
			});
		}
	}
	/// <summary>
	/// Отримати фільми за тегом
	/// </summary>
	/// <param name="tagId">ID тегу</param>
	/// <param name="pageNumber">Номер сторінки</param>
	/// <param name="pageSize">Розмір сторінки</param>
	/// <response code="200">Якщо фільми успішно знайдені</response>
	/// <response code="404">Якщо не знайдено жодного фільму</response>
	[HttpGet("movies/tag/{tagId}")]
	[ProducesResponseType(typeof(CollectionResult<MediaItemByTagDto>), 200)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> GetMoviesByTag(long tagId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _mediaItemService.GetMoviesByTagAsync(tagId, pageNumber, pageSize);

		if (result.Data == null || result.Count == 0)
		{
			return NotFound(new { message = "No movies found for the specified tag." });
		}

		return Ok(result);
	}

	/// <summary>
	/// Отримати серіали за тегом
	/// </summary>
	/// <param name="tagId">ID тегу</param>
	/// <param name="pageNumber">Номер сторінки</param>
	/// <param name="pageSize">Розмір сторінки</param>
	/// <response code="200">Якщо серіали успішно знайдені</response>
	/// <response code="404">Якщо не знайдено жодного серіалу</response>
	[HttpGet("series/tag/{tagId}")]
	[ProducesResponseType(typeof(CollectionResult<MediaItemByTagDto>), 200)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> GetSeriesByTag(long tagId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
	{
		var result = await _mediaItemService.GetSeriesByTagAsync(tagId, pageNumber, pageSize);

		if (result.Data == null || result.Count == 0)
		{
			return NotFound(new { message = "No series found for the specified tag." });
		}

		return Ok(result);
	}

	/// <summary>
	/// Видалити медіаелемент за ID
	/// </summary>
	/// <param name="id">Ідентифікатор медіаелемента</param>
	/// <response code="200">Якщо медіаелемент успішно видалений</response>
	/// <response code="400">Якщо сталася помилка при запиті або медіаелемент не знайдений</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> Delete(Guid id)
	{
		var response = await _mediaItemService.DeleteMediaItemAsync(id);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Додати новий фільм/серіал
	/// </summary>
	/// <param name="dto">Об'єкт створення медіаелемента</param>
	/// <remarks>
	/// Sample request:
	/// <code>
	/// POST
	/// {
	///      "name": "Фільм #79",
	///		 "description": "Опис фільму #7",
	///		 "originalName": "Фільм №79",
	///		 "mediaItemTypeId": 1,
	///		 "statusId": 1,
	///		 "restrictedRatingId": 1,
	///		 "seoAddition": {
	///			"slug": "film-7",
	///			"metaTitle": "Фільм #7 онлайн",
	///			"description": "Це опис фільму #7",
	///			"metaDescription": "Повний опис фільму #7",
	///			"metaImagePath": "https://path-to-image.jpg"
	///		}
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо медіаелемент успішно створений</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpPost("create")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> CreateMediaItem([FromBody] CreateMediaItemDto dto)
	{
		var result = await _mediaItemService.CreateMediaItemAsync(dto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}


	/// <summary>
	/// Оновити інформацію про фільм/серіал
	/// </summary>
	/// <param name="dto">Об'єкт оновлення медіаелемента</param>
	/// <remarks>
	/// Sample request:
	/// <code>
	/// PUT
	/// {
	///     "id": "00000000-0000-0000-0000-000000000000",
	///     "name": "Оновлений фільм #7",
	///     "description": "Оновлений опис фільму #7",
	///		"originalName": "Оновлений фільм #7",
	///     "mediaItemTypeId": 2,
	///     "statusId": 3,
	///		"restrictedRatingId": 1,
	///     "seoAddition": {
	///         "slug": "updated-film-7",
	///         "metaTitle": "Оновлений фільм #7",
	///         "description": "Оновлений опис для SEO",
	///         "metaDescription": "Оновлений мета-опис",
	///         "metaImagePath": "https://path-to-new-image.jpg"
	///     }
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо медіаелемент успішно оновлений</response>
	/// <response code="400">Якщо сталася помилка при запиті або медіаелемент не знайдений</response>
	[HttpPut("update")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> UpdateMediaItem([FromBody] UpdateMediaItemDto dto)
	{
		var result = await _mediaItemService.UpdateMediaItemAsync(dto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}


	/// <summary>
	/// Пошук та фільтрація медіаелементів
	/// </summary>
	/// <param name="searchDto">Параметри пошуку та фільтрації</param>
	/// <remarks>
	/// Simple request:
	/// <code>
	/// POST
	/// {
	///     "query": "Драма",
	///     "tagIds": [1, 2],
	///     "statusId": 3,
	///     "mediaTypeId": 1,
	///     "sortBy": "ReleaseDate",
	///     "sortDescending": true,
	///     "pageNumber": 1,
	///     "pageSize": 10
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо пошук успішний</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpPost("search")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<MediaItemDto>>> SearchMediaItems([FromForm] MediaItemSearchDto searchDto, int pageNumber = 1, int pageSize = 10)
	{
		var validator = new MediaItemSearchDtoValidator();
		var validationResult = validator.Validate(searchDto);

		if (!validationResult.IsValid)
		{
			return BadRequest(new CollectionResult<MediaItemDto>
			{
				ErrorMessage = validationResult.Errors.First().ErrorMessage
			});
		}

		var response = await _mediaItemService.SearchMediaItemsAsync(searchDto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

}
