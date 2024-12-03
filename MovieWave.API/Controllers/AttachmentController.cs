using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MovieWave.Domain.Dto;
using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Asp.Versioning;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.API.UploadFileRequest;
using MovieWave.Application.Services;
using MovieWave.Domain.Dto.MediaItem;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AttachmentController : ControllerBase
{
	private readonly IAttachmentService _attachmentService;

	public AttachmentController(IAttachmentService attachmentService)
	{
		_attachmentService = attachmentService;
	}

	/// <summary>
	/// Завантажити новий attachment
	/// </summary>
	/// <param name="mediaItemId">ID медіа-елемента</param>
	/// <param name="attachmentType">Тип attachment</param>
	/// <param name="request">Дані для завантаження файлу</param>
	[HttpPost("upload")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(typeof(BaseResult<AttachmentDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(BaseResult<AttachmentDto>), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<AttachmentDto>>> UploadAttachment([FromForm] CreateAttachmentDto dto,
		IFormFile file)
	{
		if (file == null || file.Length == 0)
		{
			return BadRequest(new BaseResult<AttachmentDto>
			{
				ErrorMessage = "Сталася помилка при завантаження файла",
				ErrorCode = 400
			});
		}

		var FileDto = FileRequest.ConvertToFileDto(file);

		var result = await _attachmentService.UploadAttachmentAsync(dto, FileDto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	/// <summary>
	/// Отримати attachment за ID
	/// </summary>
	/// <param name="attachmentId">ID attachment</param>
	[HttpGet("{attachmentId}")]
	[ProducesResponseType(typeof(BaseResult<AttachmentDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(BaseResult<AttachmentDto>), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<BaseResult<AttachmentDto>>> GetAttachmentById(Guid attachmentId)
	{
		var result = await _attachmentService.GetAttachmentByIdAsync(attachmentId);

		if (result.IsSuccess)
		{
			return Ok(result);
		}

		return NotFound(result);
	}

	/// <summary>
	/// Отримати всі attachments для медіа-елемента
	/// </summary>
	/// <param name="mediaItemId">ID медіа-елемента</param>
	[HttpGet("mediaitem/{mediaItemId}")]
	[ProducesResponseType(typeof(CollectionResult<AttachmentDto>), StatusCodes.Status200OK)]
	public async Task<ActionResult<CollectionResult<AttachmentDto>>> GetAttachmentsByMediaItemId(Guid mediaItemId)
	{
		var result = await _attachmentService.GetAttachmentsByMediaItemIdAsync(mediaItemId);

		return Ok(result);
	}

	/// <summary>
	/// Оновити attachment
	/// </summary>
	/// <param name="dto">Дані для оновлення attachment</param>
	[HttpPut("update")]
	[ProducesResponseType(typeof(BaseResult<AttachmentDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(BaseResult<AttachmentDto>), StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<AttachmentDto>>> UpdateAttachment(
		[FromForm] UpdateAttachmentDto dto,
		IFormFile file)
	{
		FileDto fileDto = null;

		if (file != null && file.Length > 0)
		{
			fileDto = FileRequest.ConvertToFileDto(file);
		}

		var result = await _attachmentService.UpdateAttachmentAsync(dto, fileDto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	/// <summary>
	/// Видалити attachment за ID
	/// </summary>
	/// <param name="attachmentId">ID attachment</param>
	[HttpDelete("{attachmentId}")]
	[ProducesResponseType(typeof(BaseResult), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(BaseResult), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<BaseResult>> DeleteAttachment(Guid attachmentId)
	{
		var result = await _attachmentService.DeleteAttachmentAsync(attachmentId);

		if (result.IsSuccess)
		{
			return Ok(result);
		}

		return NotFound(result);
	}
}