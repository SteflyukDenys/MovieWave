using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.EpisodeVoice;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class EpisodeVoiceController : ControllerBase
	{
		private readonly IEpisodeVoiceService _episodeVoiceService;

		public EpisodeVoiceController(IEpisodeVoiceService episodeVoiceService)
		{
			_episodeVoiceService = episodeVoiceService;
		}

		[HttpPost("add")]
		public async Task<ActionResult<BaseResult<EpisodeVoiceDto>>> AddEpisodeVoice([FromBody] CreateEpisodeVoiceDto dto)
		{
			var result = await _episodeVoiceService.AddEpisodeVoiceAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("{episodeId}/{voiceId}")]
		public async Task<ActionResult<BaseResult>> DeleteEpisodeVoice(Guid episodeId, long voiceId)
		{
			var result = await _episodeVoiceService.DeleteEpisodeVoiceAsync(episodeId, voiceId);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpGet("episode/{episodeId}")]
		public async Task<ActionResult<CollectionResult<EpisodeVoiceDto>>> GetVoicesByEpisodeId(Guid episodeId)
		{
			var result = await _episodeVoiceService.GetVoicesByEpisodeIdAsync(episodeId);
			return Ok(result);
		}
	}
}