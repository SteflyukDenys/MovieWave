using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.API.UploadFileRequest;
using MovieWave.Application.Services;
using MovieWave.Domain.Dto.Attachment;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Dto.Tag;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TagController : ControllerBase
{
	private readonly ITagService _tagService;
	private readonly IStorageService _storageService;

	public TagController(ITagService tagService, IStorageService storageService)
	{
		_tagService = tagService;
		_storageService = storageService;
	}

	[HttpPost("create")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<TagDto>>> CreateTag(
		[FromForm] CreateTagDto dto,
		IFormFile imageSeo)
	{
		if (imageSeo == null || imageSeo.Length == 0)
		{
			return BadRequest(new BaseResult<TagDto>
			{
				ErrorMessage = "Сталася помилка при завантаження файла",
				ErrorCode = 400
			});
		}
		var imageSeoDto = FileRequest.ConvertToFileDto(imageSeo);

		var result = await _tagService.CreateAsync(dto, imageSeoDto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	[HttpPut("update")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<TagDto>>> UpdateTag(
		[FromForm] UpdateTagDto dto,
		IFormFile newImageSeo)
	{
		FileDto imageSeoDto = null;

		if (newImageSeo != null && newImageSeo.Length > 0)
		{
			imageSeoDto = FileRequest.ConvertToFileDto(newImageSeo);
		}

		var result = await _tagService.UpdateAsync(dto, imageSeoDto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	[HttpGet("all")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<TagDto>>> GetAllTags()
	{
		var result = await _tagService.GetAllAsync();
		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<TagDto>>> GetTagById(Guid id)
	{
		var result = await _tagService.GetByIdAsync(id);
		return result.IsSuccess ? Ok(result) : NotFound(result);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult>> DeleteTag(Guid id)
	{
		var result = await _tagService.DeleteAsync(id);
		return result.IsSuccess ? Ok(result) : NotFound(result);
	}
}
