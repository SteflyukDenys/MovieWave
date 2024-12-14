using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.Episode;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class EpisodeController : ControllerBase
	{
		private readonly IEpisodeService _episodeService;

		public EpisodeController(IEpisodeService episodeService)
		{
			_episodeService = episodeService;
		}

		[HttpPost("create")]
		public async Task<ActionResult<BaseResult<EpisodeDto>>> CreateEpisode([FromBody] CreateEpisodeDto dto)
		{
			var result = await _episodeService.CreateEpisodeAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpPut("update")]
		public async Task<ActionResult<BaseResult<EpisodeDto>>> UpdateEpisode([FromBody] UpdateEpisodeDto dto)
		{
			var result = await _episodeService.UpdateEpisodeAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("{episodeId}")]
		public async Task<ActionResult<BaseResult>> DeleteEpisode(Guid episodeId)
		{
			var result = await _episodeService.DeleteEpisodeAsync(episodeId);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpGet("{episodeId}")]
		public async Task<ActionResult<BaseResult<EpisodeDto>>> GetEpisodeById(Guid episodeId)
		{
			var result = await _episodeService.GetEpisodeByIdAsync(episodeId);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpGet("mediaitem/{mediaItemId}")]
		public async Task<ActionResult<CollectionResult<EpisodeDto>>> GetEpisodesByMediaItemId(Guid mediaItemId)
		{
			var result = await _episodeService.GetEpisodesByMediaItemIdAsync(mediaItemId);
			return Ok(result);
		}
	}
}
