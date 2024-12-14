using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.Season;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class SeasonController : ControllerBase
	{
		private readonly ISeasonService _seasonService;

		public SeasonController(ISeasonService seasonService)
		{
			_seasonService = seasonService;
		}

		[HttpPost("create")]
		public async Task<ActionResult<BaseResult<SeasonDto>>> CreateSeason([FromBody] CreateSeasonDto dto)
		{
			var result = await _seasonService.CreateSeasonAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpPut("update")]
		public async Task<ActionResult<BaseResult<SeasonDto>>> UpdateSeason([FromBody] UpdateSeasonDto dto)
		{
			var result = await _seasonService.UpdateSeasonAsync(dto);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpDelete("{seasonId}")]
		public async Task<ActionResult<BaseResult>> DeleteSeason(Guid seasonId)
		{
			var result = await _seasonService.DeleteSeasonAsync(seasonId);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		[HttpGet("{seasonId}")]
		public async Task<ActionResult<BaseResult<SeasonDto>>> GetSeasonById(Guid seasonId)
		{
			var result = await _seasonService.GetSeasonByIdAsync(seasonId);
			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}
	}
}