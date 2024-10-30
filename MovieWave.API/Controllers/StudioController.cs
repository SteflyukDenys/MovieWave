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
