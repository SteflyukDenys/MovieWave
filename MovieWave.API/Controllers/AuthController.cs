using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using System.Security.Claims;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly SignInManager<User> _signInManager;

	public AuthController(IAuthService authService, SignInManager<User> signInManager)
	{
		_authService = authService;
		_signInManager = signInManager;
	}

	[HttpPost("register")]
	public async Task<ActionResult<BaseResult<UserDto>>> Register([FromBody] RegisterUserDto dto)
	{
		var response = await _authService.RegisterAsync(dto);
		return response.IsSuccess ? Ok(response) : BadRequest(response);
	}

	[HttpPost("login")]
	public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody] LoginUserDto dto)
	{
		var response = await _authService.LoginAsync(dto);
		return response.IsSuccess ? Ok(response) : BadRequest(response);
	}

	[HttpGet("google-login")]
	public IActionResult GoogleLogin(string returnUrl = null)
	{
		var redirectUrl = Url.Action(nameof(GoogleResponse), "Auth", new { returnUrl });
		var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
		properties.AllowRefresh = true;
		return Challenge(properties, GoogleDefaults.AuthenticationScheme);
	}

	[HttpGet("signin-google")]
	public async Task<IActionResult> GoogleResponse(string returnUrl = null)
	{
		var info = await _signInManager.GetExternalLoginInfoAsync();
		if (info == null)
		{
			return BadRequest("Error loading external input information.");
		}

		var email = info.Principal.FindFirstValue(ClaimTypes.Email);
		var name = info.Principal.FindFirstValue(ClaimTypes.Name);

		var response = await _authService.ExternalLoginAsync(email, name);

		if (response.IsSuccess)
		{
			await _signInManager.SignOutAsync();
			if (!string.IsNullOrEmpty(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return Ok(response);
		}

		return BadRequest(response);
	}

}
