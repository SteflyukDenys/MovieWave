using MovieWave.Domain.Dto.Comment;
using MovieWave.Domain.Result;

namespace MovieWave.Domain.Interfaces.Services
{
	public interface ICommentService
	{
		Task<BaseResult<CommentDto>> AddCommentAsync(Guid userId, AddCommentDto dto);

		Task<BaseResult<List<CommentDto>>> GetCommentsAsync(Guid mediaItemId);

		Task<BaseResult<bool>> DeleteCommentAsync(Guid userId, Guid commentId);
	}
}