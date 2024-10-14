using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.MediaItem;
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

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<MediaItemDto>>> GetMediaItem(Guid id)
    {
        var response = _mediaItemService.GetMediaItemByIdAsync(id);

        if (response.IsCompleted)
        {
            return Ok(response);
        }

       return BadRequest(response);
    }
}
