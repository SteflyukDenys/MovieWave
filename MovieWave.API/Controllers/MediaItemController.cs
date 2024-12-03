using Amazon.Runtime.Internal;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Application.Services;
using MovieWave.Application.Validations;
using MovieWave.Application.Validations.FluentValidations.MediaItem;
using MovieWave.Domain.Dto.Banner;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using MovieWave.API.UploadFileRequest;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MediaItemController : ControllerBase
{
	private readonly IMediaItemService _mediaItemService;
	private readonly IStorageService _storageService;

	public MediaItemController(IMediaItemService mediaItemService, IStorageService storageService)
	{
		_mediaItemService = mediaItemService;
		_storageService = storageService;
	}

	/// <summary>
	/// Отримати всі фільми/серіали
	/// </summary>
	/// <response code="200">Якщо список медіаелементів успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpGet("all/")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> GetMediaItemAll()
	{
		var response = await _mediaItemService.GetMediaItemsAsync();

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
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> CreateMediaItem([FromForm] CreateMediaItemDto dto)
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
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> UpdateMediaItem([FromForm] UpdateMediaItemDto dto)
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
	public async Task<ActionResult<CollectionResult<MediaItemDto>>> SearchMediaItems([FromBody] MediaItemSearchDto searchDto)
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
