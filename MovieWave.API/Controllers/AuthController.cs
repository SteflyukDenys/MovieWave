using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using System.Security.Claims;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	[HttpPost("register")]
	public async Task<ActionResult<BaseResult<UserDto>>> Register([FromBody] RegisterUserDto dto)
	{
		var response = await _authService.RegisterAsync(dto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpPost("login")]
	public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody] LoginUserDto dto)
	{
		var response = await _authService.LoginAsync(dto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpGet("google-login")]
	public IActionResult GoogleLogin()
	{
		var properties = new AuthenticationProperties
		{
			RedirectUri = Url.Action(nameof(GoogleResponse))
		};
		return Challenge(properties, GoogleDefaults.AuthenticationScheme);
	}

	[HttpGet("signin-google")]
	public async Task<IActionResult> GoogleResponse()
	{
		var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

		if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
		{
			return BadRequest("Error authenticating with Google");
		}

		var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
		var name = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name);

		var response = await _authService.ExternalLoginAsync(email, name);

		if (response.IsSuccess)
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Ok(response);
		}

		return BadRequest(response);
	}
}
