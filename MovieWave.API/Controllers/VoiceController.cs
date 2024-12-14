using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.Voice;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class VoiceController : ControllerBase
	{
		private readonly IVoiceService _voiceService;

		public VoiceController(IVoiceService voiceService)
		{
			_voiceService = voiceService;
		}

		[HttpPost("create")]
		public async Task<ActionResult<BaseResult<VoiceDto>>> CreateVoice([FromBody] CreateVoiceDto dto)
		{
			var result = await _voiceService.CreateVoiceAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpPut("update")]
		public async Task<ActionResult<BaseResult<VoiceDto>>> UpdateVoice([FromBody] UpdateVoiceDto dto)
		{
			var result = await _voiceService.UpdateVoiceAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("{voiceId}")]
		public async Task<ActionResult<BaseResult>> DeleteVoice(long voiceId)
		{
			var result = await _voiceService.DeleteVoiceAsync(voiceId);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpGet("{voiceId}")]
		public async Task<ActionResult<BaseResult<VoiceDto>>> GetVoiceById(long voiceId)
		{
			var result = await _voiceService.GetVoiceByIdAsync(voiceId);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpGet("all")]
		public async Task<ActionResult<CollectionResult<VoiceDto>>> GetAllVoices()
		{
			var result = await _voiceService.GetAllVoicesAsync();
			return Ok(result);
		}
	}
}