using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.Country;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CountryController : ControllerBase
{
	private readonly ICountryService _countryService;

	public CountryController(ICountryService countryService)
	{
		_countryService = countryService;
	}

	/// <summary>
	/// Отримати всі країни
	/// </summary>
	/// <response code="200">Якщо список країн успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpGet("all/")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<CountryDto>>> GetCountryAll()
	{
		var response = await _countryService.GetAllAsync();

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Отримати країну за ID
	/// </summary>
	/// <param name="id">Ідентифікатор країни</param>
	/// <response code="200">Якщо країна була знайдена</response>
	/// <response code="400">Якщо країна не знайдена або сталася помилка при запиті</response>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<CountryDto>>> GetCountryById(long id)
	{
		var response = await _countryService.GetByIdAsync(id);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Видалити країну за ID
	/// </summary>
	/// <param name="id">Ідентифікатор країни</param>
	/// <response code="200">Якщо країна успішно видалена</response>
	/// <response code="400">Якщо сталася помилка при запиті або країна не знайдена</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<CountryDto>>> Delete(long id)
	{
		var response = await _countryService.DeleteAsync(id);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Створити нову країну
	/// </summary>
	/// <param name="dto">Об'єкт створення країни</param>
	/// <remarks>
	/// Sample request:
	/// 
	/// <code>
	/// POST
	/// {
	///     "name": "Італія",
	///		"seoAddition": {
	///			"slug": "it",
	///			"metaTitle": "Фільми/серіали з Італії",
	///			"description": "Кращі фільми та серіали створені в Італії",
	///			"metaDescription": "Італійські популярні фільми та серіали",
	///			"metaImagePath": "https://path-to-italian-image.jpg"
	///		}
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо країна успішно створена</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<CountryDto>>> Create([FromBody] CreateCountryDto dto)
	{
		var response = await _countryService.CreateAsync(dto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Оновити інформацію про країну
	/// </summary>
	/// <param name="dto">Об'єкт оновлення країни</param>
	/// <remarks>
	/// Sample request:
	/// <code>
	/// PUT
	/// {
	///     "id": 7,
	///     "name": "Італія",
	///     "seoAddition": {
	///         "slug": "it",
	///         "metaTitle": "Італійські фільми/серіали",
	///         "description": "Кращі італійські фільми та серіали",
	///         "metaDescription": "Всі фільми доступні онлайн з Італії",
	///         "metaImagePath": "https://path-to-italian#2-image.jpg"
	///     }
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо країна успішно оновлена</response>
	/// <response code="400">Якщо сталася помилка при запиті або країна не знайдена</response>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<CountryDto>>> Update([FromBody] UpdateCountryDto dto)
	{
		var response = await _countryService.UpdateAsync(dto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}
