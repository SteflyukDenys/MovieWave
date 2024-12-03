using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.API.UploadFileRequest;
using MovieWave.Domain.Dto.Banner;
using MovieWave.Domain.Dto.S3Storage;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BannerController : ControllerBase
{
	private readonly IBannerService _bannerService;

	public BannerController(IBannerService bannerService, IStorageService storageService)
	{
		_bannerService = bannerService;
	}

	[HttpPost("create")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<BannerDto>>> CreateBanner(
		[FromForm] CreateBannerDto dto,
		IFormFile image)
	{
		if (image == null || image.Length == 0)
		{
			return BadRequest(new BaseResult<BannerDto>
			{
				ErrorMessage = "Сталася помилка при завантаження файла",
				ErrorCode = 400
			});
		}

		var imageDto = FileRequest.ConvertToFileDto(image);

		var result = await _bannerService.CreateBannerAsync(dto, imageDto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	[HttpPut("update")]
	[Consumes("multipart/form-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<BannerDto>>> UpdateBanner([FromForm] UpdateBannerDto dto, IFormFile newImage)
	{
		FileDto imageDto = null;

		if (newImage != null && newImage.Length > 0)
		{
			imageDto = FileRequest.ConvertToFileDto(newImage);
		}

		var result = await _bannerService.UpdateBannerAsync(dto, imageDto);

		return result.IsSuccess ? Ok(result) : BadRequest(result);
	}

	[HttpGet("active")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<BannerDto>>> GetActiveBanners()
	{
		var result = await _bannerService.GetActiveBannersAsync();
		return Ok(result);
	}

	[HttpGet("{bannerId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<BannerDto>>> GetBannerById(Guid bannerId)
	{
		var result = await _bannerService.GetBannerByIdAsync(bannerId);
		return result.IsSuccess ? Ok(result) : NotFound(result);
	}

	[HttpDelete("{bannerId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult>> DeleteBanner(Guid bannerId)
	{
		var result = await _bannerService.DeleteBannerAsync(bannerId);
		return result.IsSuccess ? Ok(result) : NotFound(result);
	}
}
