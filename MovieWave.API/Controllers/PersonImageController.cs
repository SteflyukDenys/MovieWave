using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MovieWave.Domain.Dto.PersonImage;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using MovieWave.API.UploadFileRequest;
using Asp.Versioning;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PersonImageController : ControllerBase
{
	private readonly IPersonImageService _personImageService;

	public PersonImageController(IPersonImageService personImageService)
	{
		_personImageService = personImageService;
	}

	/// <summary>
	/// Завантажити нове зображення для людини
	/// </summary>
	/// <param name="dto">Дані для створення зображення</param>
	/// <param name="file">Файл зображення</param>
	[HttpPost("upload")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(typeof(BaseResult<PersonImageDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(BaseResult<PersonImageDto>), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<PersonImageDto>>> UploadPersonImage([FromForm] CreatePersonImageDto dto, IFormFile file)
	{
		if (file == null || file.Length == 0)
		{
			return BadRequest(new BaseResult<PersonImageDto>
			{
				ErrorMessage = "Файл не завантажено або він порожній.",
				ErrorCode = 400
			});
		}

		var fileDto = FileRequest.ConvertToFileDto(file);

		var result = await _personImageService.UploadPersonImageAsync(dto, fileDto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	/// <summary>
	/// Отримати зображення за ID
	/// </summary>
	/// <param name="personImageId">ID зображення</param>
	[HttpGet("{personImageId}")]
	[ProducesResponseType(typeof(BaseResult<PersonImageDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(BaseResult<PersonImageDto>), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<BaseResult<PersonImageDto>>> GetPersonImageById(Guid personImageId)
	{
		var result = await _personImageService.GetPersonImageByIdAsync(personImageId);

		if (result.IsSuccess)
		{
			return Ok(result);
		}

		return NotFound(result);
	}

	/// <summary>
	/// Отримати всі зображення для людини
	/// </summary>
	/// <param name="personId">ID людини</param>
	[HttpGet("person/{personId}")]
	[ProducesResponseType(typeof(CollectionResult<PersonImageDto>), StatusCodes.Status200OK)]
	public async Task<ActionResult<CollectionResult<PersonImageDto>>> GetImagesByPersonId(Guid personId)
	{
		var result = await _personImageService.GetImagesByPersonIdAsync(personId);

		return Ok(result);
	}

	/// <summary>
	/// Оновити зображення людини
	/// </summary>
	/// <param name="dto">Дані для оновлення зображення</param>
	/// <param name="file">Новий файл зображення</param>
	[HttpPut("update")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(typeof(BaseResult<PersonImageDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(BaseResult<PersonImageDto>), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<PersonImageDto>>> UpdatePersonImage([FromForm] UpdatePersonImageDto dto, IFormFile file)
	{
		FileDto fileDto = null;

		if (file != null && file.Length > 0)
		{
			fileDto = FileRequest.ConvertToFileDto(file);
		}

		var result = await _personImageService.UpdatePersonImageAsync(dto, fileDto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	/// <summary>
	/// Видалити зображення людини
	/// </summary>
	/// <param name="personImageId">ID зображення</param>
	[HttpDelete("{personImageId}")]
	[ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(BaseResult), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<BaseResult>> DeletePersonImage(Guid personImageId)
	{
		var result = await _personImageService.DeletePersonImageAsync(personImageId);

		if (result.IsSuccess)
		{
			return Ok(result);
		}

		return NotFound(result);
	}
}
