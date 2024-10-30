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