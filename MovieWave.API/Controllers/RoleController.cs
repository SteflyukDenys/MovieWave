using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.Role;
using MovieWave.Domain.Dto.UserRole;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

//[Authorize(Roles = "Admin")]
[Consumes(MediaTypeNames.Application.Json)]
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
	private readonly IRoleService _roleService;

	public RoleController(IRoleService roleService)
	{
		_roleService = roleService;
	}

	/// <summary>
	/// Отримати всі ролі
	/// </summary>
	/// <remarks>
	/// Приклад запиту:
	/// <code>
	/// GET /api/role
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо список ролей успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<RoleDto>>> GetAll()
	{
		var response = await _roleService.GetAllRolesAsync();
		if (response.IsSuccess)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	/// <summary>
	/// Створити нову роль
	/// </summary>
	/// <param name="dto">Об'єкт даних для створення ролі</param>
	/// <remarks>
	/// Приклад запиту:
	/// <code>
	/// POST
	/// {
	///     "name": "Admin"
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо роль успішно створена</response>
	/// <response code="400">Якщо сталася помилка при запиті</response>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> Create([FromBody] CreateRoleDto dto)
	{
		var response = await _roleService.CreateRoleAsync(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	/// <summary>
	/// Оновити існуючу роль
	/// </summary>
	/// <param name="dto">Об'єкт даних для оновлення ролі</param>
	/// <remarks>
	/// Приклад запиту:
	/// <code>
	/// PUT
	/// {
	///     "id": 1,
	///     "name": "Moderator"
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо роль успішно оновлена</response>
	/// <response code="400">Якщо роль не знайдена або сталася помилка при запиті</response>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> Update([FromBody] RoleDto dto)
	{
		var response = await _roleService.UpdateRoleAsync(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	/// <summary>
	/// Видалити роль за ідентифікатором
	/// </summary>
	/// <param name="id">Ідентифікатор ролі для видалення</param>
	/// <remarks>
	/// Приклад запиту:
	/// <code>
	/// DELETE /api/role/1
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо роль успішно видалена</response>
	/// <response code="400">Якщо роль не знайдена або сталася помилка при запиті</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> Delete(long id)
	{
		var response = await _roleService.DeleteRoleAsync(id);
		if (response.IsSuccess)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	/// <summary>
	/// Додати роль користувачу
	/// </summary>
	/// <param name="dto">Об'єкт даних для створення ролі</param>
	/// <remarks>
	/// Приклад запиту:
	/// <code>
	/// POST
	/// {
	///     "login": "User #1",
	///		"roleName": "Admin"
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо роль була додана</response>
	/// <response code="400">Якщо роль не була додана</response>
	[HttpPost("add-role")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> AddRoleForUser([FromBody] UserRoleDto dto)
	{
		var response = await _roleService.AddRoleForUserAsync(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	/// <summary>
	/// Оновити роль користувачу
	/// </summary>
	/// <param name="dto">Об'єкт даних для оновлення ролі користувачу</param>
	/// <remarks>
	/// Приклад запиту:
	/// <code>
	/// PUT
	/// {
	///     
	///		"login": "User #1",
	///		"fromRoleId": 1,
	///		"toRoleId": 2
	///		
	/// 
	/// }
	/// </code>
	/// </remarks>
	/// <response code="200">Якщо роль була додана</response>
	/// <response code="400">Якщо роль не була додана</response>
	[HttpPut("update-role")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> UpdateRoleForUser([FromBody] UpdateUserRoleDto dto)
	{
		var response = await _roleService.UpdateRoleForUserAsync(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	/// <summary>
	/// Видалити роль користувача
	/// </summary>
	/// <param name="dto">Об'єкт даних для видалення ролі</param>
	/// <response code="200">Якщо роль була видалена</response>
	/// <response code="400">Якщо роль не була видалена</response>
	[HttpDelete("delete-role")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> DeleteRoleForUser([FromBody] DeleteUserRoleDto dto)
	{
		var response = await _roleService.DeleteRoleForUserAsync(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}
}
