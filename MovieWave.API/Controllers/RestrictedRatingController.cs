using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.RestrictedRating;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RestrictedRatingController : ControllerBase
{
	private readonly IRestrictedRatingService _restrictedRatingService;

	public RestrictedRatingController(IRestrictedRatingService restrictedRatingService)
	{
		_restrictedRatingService = restrictedRatingService;
	}

	/// <summary>
	/// Отримати всі вікові обмеження фільмів/серіалів
	/// </summary>
	/// <response code="200">Якщо список вікових обмежень успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpGet("all")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<RestrictedRatingDto>>> GetAll()
	{
		var response = await _restrictedRatingService.GetAllAsync();

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Отримати вікове обмеження фільмів/серіалів за ID
	/// </summary>
	/// <param name="id">Ідентифікатор вікового обмеження</param>
	/// <response code="200">Якщо вікове обмеження було знайдено</response>
	/// <response code="400">Якщо вікове обмеження не знайдено або сталася помилка при запиті</response>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<RestrictedRatingDto>>> GetById(long id)
	{
		var response = await _restrictedRatingService.GetByIdAsync(id);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Оновити інформацію про вікове обмеження фільмів/серіалів
	/// </summary>
	/// <param name="dto">Об'єкт оновлення вікового обмеження</param>
	/// <remarks>
	/// Sample request:
	/// <code>
	/// PUT
	/// {
	///     "id": 3,
	///     "name": "Parents Strongly Cautioned",
	///     "slug": "pg-13",
	///     "value": 13,
	///     "hint": "Не рекомендовано для дітей до 13 років без супроводу дорослих"
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо вікове обмеження успішно оновлено</response>
	/// <response code="400">Якщо сталася помилка при запиті або вікове обмеження не знайдено</response>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<RestrictedRatingDto>>> Update([FromBody] UpdateRestrictedRatingDto dto)
	{
		var response = await _restrictedRatingService.UpdateAsync(dto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}
