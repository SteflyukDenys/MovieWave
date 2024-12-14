using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieWave.Application.Resources;
using MovieWave.Domain.Dto.Comment;
using MovieWave.Domain.Entity;
using MovieWave.Domain.Enum;
using MovieWave.Domain.Interfaces.Databases;
using MovieWave.Domain.Interfaces.Repositories;
using MovieWave.Domain.Interfaces.Services;
using MovieWave.Domain.Result;
using Serilog;
using System.Net;

namespace MovieWave.Application.Services
{
	public class CommentService : ICommentService
	{
		private readonly IBaseRepository<Comment> _commentRepository;
		private readonly IBaseRepository<MediaItem> _mediaItemRepository;
		private readonly IBaseRepository<User> _userRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;

		public CommentService(
			IBaseRepository<Comment> commentRepository,
			IBaseRepository<MediaItem> mediaItemRepository,
			IBaseRepository<User> userRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			ILogger logger)
		{
			_commentRepository = commentRepository;
			_mediaItemRepository = mediaItemRepository;
			_userRepository = userRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public async Task<BaseResult<CommentDto>> AddCommentAsync(Guid userId, AddCommentDto dto)
		{
			var mediaItem = await _mediaItemRepository.GetAll()
				.FirstOrDefaultAsync(mi => mi.Id == dto.MediaItemId);
			if (mediaItem == null)
			{
				return new BaseResult<CommentDto>
				{
					ErrorMessage = ErrorMessage.MediaItemNotFound,
					ErrorCode = (int)ErrorCodes.MediaItemNotFound
				};
			}

			Comment? parentComment = null;
			if (dto.ParentId.HasValue)
			{
				parentComment = await _commentRepository.GetAll()
					.FirstOrDefaultAsync(c => c.Id == dto.ParentId.Value);
				if (parentComment == null)
				{
					return new BaseResult<CommentDto>
					{
						ErrorMessage = ErrorMessage.ParentCommentNotFound,
						ErrorCode = (int)ErrorCodes.ParentCommentNotFound
					};
				}
			}

			var comment = new Comment
			{
				CommentableId = dto.MediaItemId,
				CommentableType = "MediaItem",
				UserId = userId,
				Text = dto.Text,
				ParentId = dto.ParentId
			};

			await _commentRepository.CreateAsync(comment);
			await _unitOfWork.SaveChangesAsync();

			var commentDto = _mapper.Map<CommentDto>(comment);
			return new BaseResult<CommentDto>
			{
				Data = commentDto
			};
		}

		public async Task<BaseResult<List<CommentDto>>> GetCommentsAsync(Guid mediaItemId)
		{
			var comments = await _commentRepository.GetAll()
				.Where(c => c.CommentableId == mediaItemId && c.CommentableType == "MediaItem" && c.ParentId == null)
				.Include(c => c.Children)
				.Include(c => c.User)
				.ToListAsync();

			var commentDtos = _mapper.Map<List<CommentDto>>(comments);
			return new BaseResult<List<CommentDto>>
			{
				Data = commentDtos
			};
		}

		public async Task<BaseResult<bool>> DeleteCommentAsync(Guid userId, Guid commentId)
		{
			var comment = await _commentRepository.GetAll()
				.FirstOrDefaultAsync(c => c.Id == commentId);

			if (comment == null)
			{
				return new BaseResult<bool>
				{
					ErrorMessage = ErrorMessage.CommentNotFound,
					ErrorCode = (int)ErrorCodes.CommentNotFound
				};
			}

			if (comment.UserId != userId)
			{
				return new BaseResult<bool>
				{
					ErrorMessage = ErrorMessage.Unauthorized,
					ErrorCode = (int)HttpStatusCode.Unauthorized
				};
			}

			_commentRepository.Remove(comment);
			await _unitOfWork.SaveChangesAsync();

			return new BaseResult<bool>
			{
				Data = true
			};
		}
	}
}
