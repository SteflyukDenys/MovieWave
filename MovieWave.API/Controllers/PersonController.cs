using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.Person;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PersonController : ControllerBase
{
	private readonly IPersonService _personService;

	public PersonController(IPersonService personService)
	{
		_personService = personService;
	}

	[HttpPost("create")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<PersonDto>>> CreatePerson([FromForm] CreatePersonDto dto)
	{
		var result = await _personService.CreatePersonAsync(dto);
		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	[HttpPut("update")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<PersonDto>>> UpdatePerson([FromForm] UpdatePersonDto dto)
	{
		var result = await _personService.UpdatePersonAsync(dto);
		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	[HttpGet("{personId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<BaseResult<PersonDto>>> GetPersonById(Guid personId)
	{
		var result = await _personService.GetPersonByIdAsync(personId);
		return result.IsSuccess ? Ok(result) : NotFound(result);
	}

	[HttpGet("all")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<CollectionResult<PersonDto>>> GetAllPersons()
	{
		var result = await _personService.GetAllPersonsAsync();
		return Ok(result);
	}

	[HttpDelete("{personId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<BaseResult>> DeletePerson(Guid personId)
	{
		var result = await _personService.DeletePersonAsync(personId);
		return result.IsSuccess ? Ok(result) : NotFound(result);
	}
}
