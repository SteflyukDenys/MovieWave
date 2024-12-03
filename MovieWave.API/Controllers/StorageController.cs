using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class StorageController : ControllerBase
{
	private readonly IStorageService _storageService;

	public StorageController(IStorageService storageService)
	{
		_storageService = storageService;
	}

	/// <summary>
	/// Завантажити файл
	/// </summary>
	/// <param name="request">Параметри завантаження файлу</param>
	/// <returns>Ключ завантаженого файлу</returns>
	[HttpPost("upload")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(typeof(BaseResult<string>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(BaseResult<string>), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<string>>> UploadFile(FormFile request, string folder)
	{
		if (request == null || request.Length == 0)
		{
			return BadRequest(new BaseResult<string>
			{
				ErrorMessage = "Файл не завантажено.",
				ErrorCode = 400
			});
		}

		var fileDto = new FileDto
		{
			FileName = request.FileName,
			Content = request.OpenReadStream(),
			ContentType = request.ContentType
		};

		var response = await _storageService.UploadFileAsync(fileDto, folder);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}


	/// <summary>
	/// Отримати файл з S3 за ключем
	/// </summary>
	/// <param name="key">Ключ файлу в S3</param>
	/// <response code="200">Якщо файл успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при отриманні файлу</response>
	[HttpGet("download")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetFile([FromQuery] string key)
	{
		var response = await _storageService.GetFileAsync(key);

		if (response.IsSuccess)
		{
			return File(response.Data, "application/octet-stream", key);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Видалити файл з S3 за ключем
	/// </summary>
	/// <param name="key">Ключ файлу в S3</param>
	/// <response code="200">Якщо файл успішно видалено</response>
	/// <response code="400">Якщо сталася помилка при видаленні файлу</response>
	[HttpDelete("delete")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult>> DeleteFile([FromQuery] string key)
	{
		var response = await _storageService.DeleteFileAsync(key);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	/// Отримати список файлів з S3
	/// </summary>
	/// <param name="prefix">Префікс для фільтрації файлів</param>
	/// <response code="200">Якщо список файлів успішно отримано</response>
	/// <response code="400">Якщо сталася помилка при отриманні списку файлів</response>
	[HttpGet("list")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<S3ObjectDto>>> ListFiles([FromQuery] string? prefix)
	{
		var response = await _storageService.ListFilesAsync(prefix);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
};
