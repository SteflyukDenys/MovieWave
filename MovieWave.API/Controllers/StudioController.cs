using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.Studio;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class StudioController : ControllerBase
{
	private readonly IStudioService _studioService;

	public StudioController(IStudioService studioService)
	{
		_studioService = studioService;
	}

	/// <summary>
	/// Отримати всі студії фільмів/серіалів
	/// </summary>
	/// <response code="200">Якщо список студій успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpGet("all")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<StudioDto>>> GetAll()
	{
		var response = await _studioService.GetAllAsync();
		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Отримати студію фільмів/серіалів за ID
	/// </summary>
	/// <param name="id">Ідентифікатор студії</param>
	/// <response code="200">Якщо студію було знайдено</response>
	/// <response code="400">Якщо студію не знайдено або сталася помилка при запиті</response>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<StudioDto>>> GetById(long id)
	{
		var response = await _studioService.GetByIdAsync(id);
		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Додати нову студію фільмів/серіалів
	/// </summary>
	/// <param name="dto">Об'єкт створення студії</param>
	/// <remarks>
	/// Sample request:
	/// <code>
	/// POST
	/// {
	///     "name": "Disney",
	///     "description": "Світовий лідер у виробництві анімаційних фільмів",
	///     "logoPath": "https://path-to-disney-logo.jpg",
	///     "seoAddition": {
	///         "slug": "disney",
	///         "metaTitle": "Фільми Disney",
	///         "description": "Сімейні та анімаційні фільми від Disney",
	///         "metaDescription": "Класичні та нові фільми Disney",
	///         "metaImagePath": "https://path-to-disney-image.jpg"
	///     }
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо студію успішно створено</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<StudioDto>>> Create([FromBody] CreateStudioDto dto)
	{
		var response = await _studioService.CreateAsync(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Оновити інформацію про студію фільмів/серіалів
	/// </summary>
	/// <param name="dto">Об'єкт оновлення студії</param>
	/// <remarks>
	/// Sample request:
	/// <code>
	/// PUT
	/// {
	///     "id": 1,
	///     "name": "Universal Pictures",
	///     "description": "Студія з відомими світовими фільмами",
	///     "logoPath": "https://path-to-universal-logo.jpg",
	///     "seoAddition": {
	///         "slug": "universal-pictures",
	///         "metaTitle": "Фільми Universal Pictures",
	///         "description": "Відомі фільми від Universal Pictures",
	///         "metaDescription": "Переглядайте популярні фільми Universal Pictures",
	///         "metaImagePath": "https://path-to-universal-image.jpg"
	///     }
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо студію успішно оновлено</response>
	/// <response code="400">Якщо сталася помилка при запиті або студію не знайдено</response>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<StudioDto>>> Update([FromBody] UpdateStudioDto dto)
	{
		var response = await _studioService.UpdateAsync(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Видалити студію фільмів/серіалів за ID
	/// </summary>
	/// <param name="id">Ідентифікатор студії</param>
	/// <response code="200">Якщо студію успішно видалено</response>
	/// <response code="400">Якщо сталася помилка при запиті або студію не знайдено</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<StudioDto>>> Delete(long id)
	{
		var response = await _studioService.DeleteAsync(id);
		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}
