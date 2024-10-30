using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.Status;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class StatusController : ControllerBase
{
	private readonly IStatusService _statusService;

	public StatusController(IStatusService statusService)
	{
		_statusService = statusService;
	}

	/// <summary>
	/// Отримати всі статуси фільмів/серіалів
	/// </summary>
	/// <response code="200">Якщо список статусів успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpGet("all")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<StatusDto>>> GetAllStatuses()
	{
		var response = await _statusService.GetAllAsync();

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Отримати статус фільмів/серіалів за ID
	/// </summary>
	/// <param name="id">Ідентифікатор статуса</param>
	/// <response code="200">Якщо статус був знайдений</response>
	/// <response code="400">Якщо статус не знайдено або сталася помилка при запиті</response>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<StatusDto>>> GetStatusById(long id)
	{
		var response = await _statusService.GetByIdAsync(id);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Оновити інформацію про статус фільмів/серіалів
	/// </summary>
	/// <param name="dto">Об'єкт оновлення статусу</param>
	/// <remarks>
	/// Sample request:
	/// <code>
	/// PUT
	/// {
	///     "id": 3,
	///     "statusType": "Ongoing",
	///     "description": "Проект наразі у процесі реалізації"
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо статус успішно оновлено</response>
	/// <response code="400">Якщо сталася помилка при запиті або статус не знайдено</response>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<StatusDto>>> UpdateStatus([FromBody] UpdateStatusDto dto)
	{
		var response = await _statusService.UpdateAsync(dto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}
