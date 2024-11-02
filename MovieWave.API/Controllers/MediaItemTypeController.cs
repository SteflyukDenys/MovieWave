using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.MediaItemType;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class MediaItemTypeController : ControllerBase
{
	private readonly IMediaItemTypeService _mediaItemTypeService;

	public MediaItemTypeController(IMediaItemTypeService mediaItemTypeService)
	{
		_mediaItemTypeService = mediaItemTypeService;
	}

	/// <summary>
	/// Отримати всі типи медіаелементів
	/// </summary>
	/// <response code="200">Якщо список типів медіаелементів успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpGet("all/")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<MediaItemTypeDto>>> GetAll()
	{
		var response = await _mediaItemTypeService.GetAllAsync();

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Отримати тип медіаелемента за ID
	/// </summary>
	/// <param name="id">Ідентифікатор типу медіаелемента</param>
	/// <response code="200">Якщо тип медіаелемента був знайдений</response>
	/// <response code="400">Якщо тип медіаелемента не знайдено або сталася помилка при запиті</response>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemTypeDto>>> GetById(int id)
	{
		var response = await _mediaItemTypeService.GetByIdAsync(id);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}