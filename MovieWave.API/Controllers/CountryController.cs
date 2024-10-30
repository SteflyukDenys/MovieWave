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