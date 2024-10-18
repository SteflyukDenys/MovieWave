using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.MediaItemType;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class MediaItemTypeController : ControllerBase
	{
		private readonly IMediaItemTypeService _mediaItemTypeService;

		public MediaItemTypeController(IMediaItemTypeService mediaItemTypeService)
		{
			_mediaItemTypeService = mediaItemTypeService;
		}

		[HttpGet("all/")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<CollectionResult<MediaItemTypeDto>>> GetAll()
		{
			var response = await _mediaItemTypeService.GetAllAsync();

			if (response.IsSuccess)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BaseResult<MediaItemTypeDto>>> GetById(int id)
		{
			var response = await _mediaItemTypeService.GetByIdAsync(id);

			if (response.IsSuccess)
			{
				return Ok(response);
			}

			return BadRequest(response);
		}
	}
}
