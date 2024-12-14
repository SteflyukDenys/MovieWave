using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieWave.Domain.Dto.Comment;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using System.Security.Claims;

namespace MovieWave.API.Controllers
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/comments")]
	[Authorize]
	public class CommentsController : ControllerBase
	{
		private readonly ICommentService _commentService;

		public CommentsController(ICommentService commentService)
		{
			_commentService = commentService;
		}

		/// <summary>
		/// Додати коментар до медіаелемента
		/// </summary>
		/// <param name="dto">Об'єкт коментаря</param>
		/// <response code="200">Якщо коментар успішно додано</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BaseResult<CommentDto>>> AddComment([FromBody] AddCommentDto dto)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var result = await _commentService.AddCommentAsync(userId, dto);

			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}

		/// <summary>
		/// Отримати коментарі для медіаелемента
		/// </summary>
		/// <param name="mediaItemId">ID медіаелемента</param>
		/// <response code="200">Якщо коментарі успішно отримані</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpGet("media/{mediaItemId}")]
		[ProducesResponseType(typeof(BaseResult<List<CommentDto>>), 200)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetComments(Guid mediaItemId)
		{
			var result = await _commentService.GetCommentsAsync(mediaItemId);

			if (!result.IsSuccess)
			{
				return BadRequest(result);
			}

			return Ok(result);
		}

		/// <summary>
		/// Видалити коментар
		/// </summary>
		/// <param name="commentId">ID коментаря</param>
		/// <response code="200">Якщо коментар успішно видалено</response>
		/// <response code="400">Якщо виникла помилка</response>
		[HttpDelete("{commentId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> DeleteComment(Guid commentId)
		{
			var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var result = await _commentService.DeleteCommentAsync(userId, commentId);

			return result.IsSuccess ? Ok(result) : BadRequest(result);
		}
	}
}
