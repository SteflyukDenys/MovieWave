using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWave.API.UploadFileRequest;
using MovieWave.Domain.Dto.User;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using System.Security.Claims;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/account")]
	[Authorize]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		/// <summary>
		/// Отримати інформацію про поточного користувача
		/// </summary>
		/// <response code="200">Якщо інформація успішно отримана</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpGet("me")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BaseResult<UserDto>>> GetCurrentUser()
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var user = await _accountService.GetUserByIdAsync(userId);
			if (user == null)
			{
				return BadRequest(new BaseResult<UserDto>
				{
					ErrorMessage = "User not found.",
					ErrorCode = (int)ErrorCodes.UserNotFound
				});
			}
			return Ok(user);
		}

		/// <summary>
		/// Оновити інформацію про профіль
		/// </summary>
		/// <param name="dto">Об'єкт оновлення профілю</param>
		/// <response code="200">Якщо оновлення успішне</response>
		/// <response code="400">Якщо виникла помилка при оновленні</response>
		[HttpPut("update")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BaseResult<UserDto>>> UpdateProfile([FromBody] UpdateUserDto dto)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var result = await _accountService.UpdateProfileAsync(userId, dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		/// <summary>
		/// Оновити аватарку користувача
		/// </summary>
		/// <param name="dto">Об'єкт аватарки</param>
		/// <response code="200">Якщо аватарка успішно оновлена</response>
		/// <response code="400">Якщо виникла помилка при завантаженні</response>
		[HttpPost("update-avatar")]
		[Consumes("multipart/form-data")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BaseResult<string>>> UpdateAvatar(IFormFile avatar)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			if (avatar == null || avatar.Length == 0)
			{
				return BadRequest(new BaseResult<string>
				{
					ErrorMessage = "Сталася помилка при завантаження файла",
					ErrorCode = 400
				});
			}

			var imageDto = FileRequest.ConvertToFileDto(avatar);

			var result = await _accountService.UpdateAvatarAsync(userId, imageDto);

			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}
	}
}
