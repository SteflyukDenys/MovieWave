using Microsoft.AspNetCore.Mvc;
using MovieWave.Application.Services;
using MovieWave.Domain.Dto;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

/// <summary>
/// 
/// </summary>

[ApiController]
public class TokenController : Controller
{
	private readonly ITokenGeneratorService _tokenGeneratorService;

	public TokenController(ITokenGeneratorService tokenGeneratorService)
	{
		_tokenGeneratorService = tokenGeneratorService;
	}

	[HttpPost]
	[Route("refresh")]
	public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody] TokenDto tokenDto)
	{
		var response = await _tokenGeneratorService.RefreshToken(tokenDto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}