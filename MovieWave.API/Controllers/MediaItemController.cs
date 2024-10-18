using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.MediaItem;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class MediaItemController : ControllerBase
{
    private readonly IMediaItemService _mediaItemService;

    public MediaItemController(IMediaItemService mediaItemService)
    {
        _mediaItemService = mediaItemService;
    }

	[HttpGet("all/")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> GetMediaItemAll()
	{
		var response = await _mediaItemService.GetMediaItemsAsync();

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> GetMediaItemById(Guid id)
	{
		var response = await _mediaItemService.GetMediaItemByIdAsync(id);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> Delete(Guid id)
	{
		var response = await _mediaItemService.DeleteMediaItemAsync(id);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> Create([FromBody] CreateMediaItemDto dto)
	{
		var response = await _mediaItemService.CreateMediaItemAsync(dto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<MediaItemDto>>> Update([FromBody] UpdateMediaItemDto dto)
	{
		var response = await _mediaItemService.UpdateMediaItemAsync(dto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}
