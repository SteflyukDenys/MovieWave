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
[ApiVersion("1.0")]
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

	/// <summary>
	/// Реєстрація нового користувача
	/// </summary>
	/// <remarks>
	/// Sample request:
	/// <code>
	/// POST /api/v2.0/auth/register
	/// {
	///     "login": "user123",
	///     "email": "user@example.com",
	///     "password": "password123",
	///     "passwordConfirm": "password123"
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо реєстрація пройшла успішно</response>
	/// <response code="400">Якщо виникла помилка при реєстрації</response>
	[HttpPost("register")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<UserDto>>> Register([FromBody] RegisterUserDto dto)
	{
		var response = await _authService.RegisterAsync(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	/// <summary>
	/// Вхід користувача
	/// </summary>
	/// <remarks>
	/// Sample request:
	/// <code>
	/// POST /api/v2.0/auth/login
	/// {
	///     "login": "user123",
	///     "password": "password123"
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо вхід успішний</response>
	/// <response code="400">Якщо логін або пароль некоректні</response>
	[HttpPost("login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody] LoginUserDto dto)
	{
		var response = await _authService.LoginAsync(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Вхід через Google
	/// </summary>
	/// <remarks>
	/// <code>
	/// GET /api/v2.0/auth/google-login
	/// </code>
	/// </remarks>
	/// <response code="302">Переадресація на сторінку входу Google</response>
	[HttpGet("google-login")]
	[ProducesResponseType(StatusCodes.Status302Found)]
	public IActionResult GoogleLogin(string returnUrl = null)
	{
		var redirectUrl = Url.Action(nameof(GoogleResponse), "Auth", new { returnUrl });
		var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
		properties.AllowRefresh = true;
		return Challenge(properties, GoogleDefaults.AuthenticationScheme);
	}

	/// <summary>
	/// Обробка відповіді від Google після авторизації
	/// </summary>
	/// <param name="returnUrl">URL для повернення після успішного входу</param>
	/// <remarks>
	/// <code>
	/// GET /api/v2.0/auth/signin-google
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо вхід через Google успішний</response>
	/// <response code="400">Якщо виникла помилка при обробці авторизації</response>
	[HttpGet("signin-google")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
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
			if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return Ok(response);
		}

		return BadRequest(response);
	}
}
